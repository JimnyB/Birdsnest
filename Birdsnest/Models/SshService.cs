using Renci.SshNet;

public class SshService
{
    private string _host;
    private string _username;
    private string _password;

    public SshService(string host, string username, string password)
    {
        _host = host;
        _username = username;
        _password = password;
    }

    public string ExecuteCommand(string command)
    {
        using (var client = new SshClient(_host, _username, _password))
        {
            client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(30);

            client.Connect();
            var cmd = client.CreateCommand(command);
            cmd.CommandTimeout = TimeSpan.FromSeconds(30);

            var result = cmd.Execute();
            var error = cmd.Error;

            client.Disconnect();

            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception($"Command execution error: {error}");
            }

            return result;
        }
    }

    public void CopyFileToLocal(string remoteFilePath, string localFilePath)
    {
        using (var scpClient = new ScpClient(_host, _username, _password))
        {
            scpClient.ConnectionInfo.Timeout = TimeSpan.FromSeconds(30);

            scpClient.Connect();
            using (var localFile = System.IO.File.Create(localFilePath))
            {
                scpClient.Download(remoteFilePath, localFile);
            }
            scpClient.Disconnect();
        }
    }
}