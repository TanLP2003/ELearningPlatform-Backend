using ApiGateway.Utils;
using Yarp.ReverseProxy.Configuration;

namespace ApiGateway.Configurations;

public partial class YarpConfig
{
    public static IReadOnlyList<ClusterConfig> GetClusters(IConfiguration configuration)
    {
        var cluster = new List<ClusterConfig>
        {
            // AUTH SERVICE
            new ClusterConfig
            {
                ClusterId = ClusterId.AuthService,
                Destinations = CreateSingleDestination(new DestinationConfig
                {
                    Address = configuration["Urls:AuthService"]
                })
            },
            // VIDEO MANAGER
            new ClusterConfig
            {
                ClusterId = ClusterId.VideoManager,
                Destinations = CreateSingleDestination(new DestinationConfig
                {
                    Address = configuration["Urls:VideoManager"]
                })
            },
            // COURSE MANAGER
            new ClusterConfig
            {
                ClusterId = ClusterId.CourseManager,
                Destinations = CreateSingleDestination(new DestinationConfig
                {
                    Address = configuration["Urls:CourseManager"]
                })
            },
            // VIDEO LIBRARY
            new ClusterConfig
            {
                ClusterId = ClusterId.VideoLibrary,
                Destinations = CreateSingleDestination(new DestinationConfig
                {
                    Address = configuration["Urls:VideoLibrary"]
                })
            },
            // LEARNING SERVICE
            new ClusterConfig
            {
                ClusterId = ClusterId.LearningService,
                Destinations = CreateSingleDestination(new DestinationConfig
                {
                    Address = configuration["Urls:LearningService"]
                })
            },
            // WISH LIST
            new ClusterConfig
            {
                ClusterId = ClusterId.WishList,
                Destinations = CreateSingleDestination(new DestinationConfig
                {
                    Address = configuration["Urls:WishList"]
                })
            },
            // BASKET
            new ClusterConfig
            {
                ClusterId = ClusterId.Basket,
                Destinations = CreateSingleDestination(new DestinationConfig
                {
                    Address = configuration["Urls:Basket"]
                })
            },
            // ORDERING
            //new ClusterConfig
            //{
            //    ClusterId = ClusterId.Ordering,
            //    Destinations = CreateSingleDestination(new DestinationConfig
            //    {
            //        Address = configuration["Urls:Ordering"]
            //    })
            //},
            // USER_SERVICE
            new ClusterConfig
            {
                ClusterId = ClusterId.UserService,
                Destinations = CreateSingleDestination(new DestinationConfig
                {
                    Address = configuration["Urls:UserService"]
                })
            },
            //PAYMENT_SERVICE
            new ClusterConfig
            {
                ClusterId = ClusterId.PaymentService,
                Destinations = CreateSingleDestination(new DestinationConfig
                {
                    Address = configuration["Urls:PaymentService"]
                })
            }
        };
        return cluster.AsReadOnly();
    }

    public static Dictionary<string, DestinationConfig> CreateSingleDestination(DestinationConfig destination)
    {
        return new Dictionary<string, DestinationConfig>()
        {
            { "destination1", destination }
        };
    }
}