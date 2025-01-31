using Application.Messaging;
using MediatR;

namespace VideoManager.API.Application.Commands.RevertVideoProcess;

public class RevertVideoCommandHandler(ILogger<RevertVideoCommandHandler> logger) : ICommandHandler<RevertVideoProcessCommand>
{
    public async Task<Unit> Handle(RevertVideoProcessCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("========> TRYING TO REVERT VIDEO PROCESS");
        try
        {
            Directory.Delete(request.DownloadFolder, true);
            logger.LogInformation("======> SUCCESS TO REVERT VIDEO PROCESS");
            logger.LogInformation("========> DELETE VIDEO FOLDER OF LECTURE");
            return Unit.Value;
        }
        catch(Exception ex)
        {
            logger.LogError("========> FAILED TO REVERT VIDEO PROCESS");
            return Unit.Value;
        }
    }
}