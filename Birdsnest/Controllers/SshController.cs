using Birdsnest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

public class SshController : Controller
{
    private readonly SshService _sshService;

    public SshController()
    {
        // Initialize the SSH service with your Raspberry Pi credentials
        _sshService = new SshService("192.168.0.34", "owlcam", "227727");
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Connect()
    {
        // Run a command to test the SSH connection
        string result = _sshService.ExecuteCommand("echo 'SSH connection successful!'");
        ViewBag.Result = result;
        return View("Index");
    }

    [HttpPost]
    public IActionResult ExecuteTerminalCommand(string command)
    {
        try
        {
            // Execute the custom command using the SSH service
            var result = _sshService.ExecuteCommand(command);

            // Pass the result back to the view
            ViewBag.Result = $"Command executed successfully:\n{result}";
        }
        catch (Exception ex)
        {
            ViewBag.Result = $"An error occurred: {ex.Message}";
        }

        return View("Index");
    }


    [HttpPost]
    public IActionResult TakePicture()
    {
        try
        {
            // Command to execute the Python script
            string command = "python3 /home/owlcam/OwlRepo/capture_image.py";
            var result = _sshService.ExecuteCommand(command);

            ViewBag.Result = $"Script executed successfully: {result}";
        }
        catch (Exception ex)
        {
            ViewBag.Result = $"An error occurred: {ex.Message}";
        }

        return View("Index");
    }

    [HttpPost]
    public IActionResult CopyFile()
    {
        try
        {
            string remoteFilePath = "/home/owlcam/OwlRepo/Photos/output_image.jpg";
            string localFilePath = @"C:\Users\Jimmy\OneDrive\Desktop\Owlcam\output_image.jpg";

            // Use the new CopyFileToLocal method
            _sshService.CopyFileToLocal(remoteFilePath, localFilePath);

            ViewBag.Result = $"File copied successfully! Saved to {localFilePath}.";
        }
        catch (Exception ex)
        {
            ViewBag.Result = $"An error occurred during file copy: {ex.Message}";
        }

        return View("Index");
    }
}