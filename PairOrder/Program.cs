// See https://aka.ms/new-console-template for more information
using PairOrder.Models.Request;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Text.Json;
using PairOrder.Models.Response;
using PairOrder;

#region configuration Values
//Setting up the config manager.
IConfiguration config = new ConfigurationBuilder()
.SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
.AddJsonFile("appsettings.json")
.Build();

var apiKey = config.GetSection("apiKey").Value;
var baseUrl = config.GetSection("baseUrl").Value;
var getEncryptionUrl = config.GetSection("getEncryptionUrl").Value;
var getSessionUrl = config.GetSection("getSessionUrl").Value;
var placeOrderUrl = config.GetSection("placeOrderUrl").Value;
var userId = config.GetSection("userId").Value;
var nifty = config.GetSection("scriptNf").Value;
var banknifty = config.GetSection("scriptBnf").Value;
int quantitySize = Convert.ToInt32(config.GetSection("quantity").Value);
var straddleType = config.GetSection("straddleType").Value;

# endregion

//creating a RestClient using a third party library
var client = new RestClient(baseUrl);

//GetEncryptionKey
string encryptedKey = GetEncrptedKey(client);

//GetSessionId
string sessionId = GetSessionId(client, encryptedKey);

Console.WriteLine("Enter the ATM Strike : ");
string atmStrike = Console.ReadLine();

string bnfCeStrike = $"{Helper.GetBankNiftyExpiryScript(config, banknifty)}{atmStrike}CE";
string bnfPeStrike = $"{Helper.GetBankNiftyExpiryScript(config, banknifty)}{atmStrike}PE";


try
{
    //GetSymbol_Id

    var symbolIdCE = GetSymbolToken(bnfCeStrike);
    var symbolIdPE = GetSymbolToken(bnfPeStrike);

    //PlaceOrder

    var response = PlaceOrder(bnfCeStrike, bnfPeStrike, symbolIdCE, symbolIdPE);
    Console.WriteLine(response);
    Console.ReadLine();

}
catch (Exception ex)
{
    LogExceptiontoConsole(ex);
}
#region Local Methods

/// <summary>
/// Gets the sessionId
/// </summary>
string GetSessionId(RestClient client, string encryptedKey)
{
    try
    {
        var sessionRequest = new RestRequest(baseUrl + getSessionUrl, Method.Post);
        sessionRequest.AddHeader("Content-Type", "application/json");
        var hashedUserData = Helper.ComputeSha256Hash(userId + apiKey + encryptedKey);
        var sessionBody = new GetSessionIdModel { userId = userId, userData = hashedUserData };
        sessionRequest.AddParameter("application/json", JsonSerializer.Serialize(sessionBody), ParameterType.RequestBody);
        return JsonSerializer.Deserialize<GetSessionIdResponse>(client.Execute(sessionRequest).Content).sessionID;
    }
    catch (Exception ex)
    {
        LogExceptiontoConsole(ex);
        throw;
    }
}

/// <summary>
/// Gets the EncryptedKey
/// </summary>
string GetEncrptedKey(RestClient client)
{
    try
    {
        var restRequest = new RestRequest(baseUrl + getEncryptionUrl, Method.Post);
        restRequest.AddHeader("Content-Type", "application/json");
        var body = new GetEncryptionModel { userId = userId };
        restRequest.AddParameter("application/json", JsonSerializer.Serialize(body), ParameterType.RequestBody);
        return JsonSerializer.Deserialize<EncrytedKeyResponse>(client.Execute(restRequest).Content).encKey;
    }
    catch (Exception ex)
    {
        LogExceptiontoConsole(ex);
        throw;
    }
}

/// <summary>
/// Gets the symbol_Id
/// </summary>
string GetSymbolToken(string scriptWithStrike)
{
    try
    {
        var scriptSearchRequest = new RestRequest(config.GetSection("v1UrlGetScript").Value, Method.Post);
        scriptSearchRequest.AddHeader("Content-Type", "application/json");
        scriptSearchRequest.AddHeader("Authorization", $"Bearer {userId} {sessionId}");

        var scripBody = new GetScripModel { symbol = scriptWithStrike, exchange = new List<string> { "NFO" } };
        scriptSearchRequest.AddParameter("application/json", JsonSerializer.Serialize(scripBody), ParameterType.RequestBody);

        var response = client.Execute(scriptSearchRequest);
        return JsonSerializer.Deserialize<List<GetScriptToken>>(response.Content)[0].token;
    }
    catch (Exception ex)
    {
        LogExceptiontoConsole(ex);
        throw;
    }
}

/// <summary>
/// Prepares the order Payload
/// </summary>
PlaceOrder OrderPayload(string symbol, string symbolId, int quantitySize, string straddleType)
{
    return new PlaceOrder
    {
        complexty = "REGULAR",
        discqty = "0",
        exch = "NFO",
        pCode = "NRML",
        prctyp = "MKT",
        price = "",
        qty = quantitySize,
        ret = "DAY",
        symbol_id = symbolId,
        trading_symbol = symbol,
        transtype = straddleType,
        trigPrice = ""
    };
}

/// <summary>
/// Places the market order
/// </summary>
string PlaceOrder(string bnfCeStrike, string bnfPeStrike, string symbolIdCE, string symbolIdPE)
{
    try
    {
        var orderRequest = new RestRequest(baseUrl + placeOrderUrl, Method.Post);
        orderRequest.AddHeader("Authorization", $"Bearer {userId} {sessionId}");
        orderRequest.AddHeader("Content-Type", "application/json");

        PlaceOrder orderBodyCE = OrderPayload(bnfCeStrike, symbolIdCE, quantitySize, straddleType);
        PlaceOrder orderBodyPE = OrderPayload(bnfPeStrike, symbolIdPE, quantitySize, straddleType);

        var listPayload = new List<PlaceOrder> { orderBodyCE, orderBodyPE };

        #region samplePayload
        //var scripBody = "[\r\n  {\r\n   \"complexty\": \"REGULAR\",\r\n    \"discqty\": \"0\",\r\n    \"exch\": \"NFO\",\r\n    \"pCode\": \"NRML\",\r\n    \"prctyp\": \"MKT\",\r\n    \"price\": \"0\",\r\n    \"qty\": 50,\r\n    \"ret\": \"DAY\",\r\n    \"symbol_id\": \"52591\",\r\n    \"trading_symbol\": \"NIFTY22SEP2217000PE\",\r\n    \"transtype\": \"SELL\",\r\n    \"trigPrice\": \"\"\r\n  }\r\n]";
        #endregion

        orderRequest.AddParameter("application/json", JsonSerializer.Serialize(listPayload), ParameterType.RequestBody);
        var response = client.Execute(orderRequest);
        return response.Content;
    }
    catch (Exception ex)
    {
        LogExceptiontoConsole(ex);
        throw;
    }
}

void LogExceptiontoConsole(Exception ex)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ResetColor();
}
#endregion