echo "preenchendo o arquivo _azurestorage.config"
echo @" 
  <appSettings>
    <add key="DiagnosticsConnectionString" value="UseDevelopmentStorage=true"/>
  </appSettings>
"@ > ".\src\QuantoEh\QuantoEh.Utils.CommandLine\_azurestorage.config"

echo "preenchendo o arquivo _twitterkeys.config"
echo @"
  <appSettings>
    <add key="twitterConsumerKey" value="coloque um consumer key aqui"/>
    <add key="twitterConsumerSecret" value="`coloque um segredo do twitter aqui"/>
    <add key="twitterOAuthToken" value="coloque um token de autorizacao aqui"/>
    <add key="twitterOAuthTokenSecret" value="coloque um segredo do oauth aqui"/>
    <add key="twitterUserID" value="coloque o id do twitter, uns 9 números, aqui"/>
    <add key="twitterScreenName" value="o nome do id do seu twitter, como por ex. quantoeh"/>
  </appSettings>
"@ > ".\src\QuantoEh\QuantoEh.Worker\_twitterkeys.config"
