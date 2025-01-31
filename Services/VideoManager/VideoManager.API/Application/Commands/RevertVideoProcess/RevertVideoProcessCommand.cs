using Application.Messaging;

namespace VideoManager.API.Application.Commands.RevertVideoProcess;

public record RevertVideoProcessCommand(string DownloadFolder) : ICommand;