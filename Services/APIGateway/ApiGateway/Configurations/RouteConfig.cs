using ApiGateway.Utils;
using Yarp.ReverseProxy.Configuration;

namespace ApiGateway.Configurations;

public partial class YarpConfig
{
    public static IReadOnlyList<RouteConfig> GetRoutes(IConfiguration configuration)
    {
        var routes = new List<RouteConfig>
        {
            // AUTH SERVICE
            new RouteConfig
            {
                RouteId = RouteId.AuthServiceRoutes,
                ClusterId = ClusterId.AuthService,
                Match = new RouteMatch
                {
                    Path = "auth/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        {"PathPattern", "{**catch-all}" }
                    }
                }
            },
            // VIDEO MANAGER
            new RouteConfig
            {
                RouteId = RouteId.VideoManagerRoutes,
                ClusterId = ClusterId.VideoManager,
                Match = new RouteMatch
                {
                    Path = "video-manager/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        {"PathPattern", "{**catch-all}" }
                    }
                },
                AuthorizationPolicy = "Default"
            },
            // COURSE MANAGER
            new RouteConfig
            {
                RouteId = RouteId.CourseManagerRoutes,
                ClusterId = ClusterId.CourseManager,
                Match = new RouteMatch
                {
                    Path = "course-manager/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        {"PathPattern", "{**catch-all}" }
                    }
                }
            },
            // VIDEO LIBRARY
            new RouteConfig
            {
                RouteId = RouteId.VideoLibraryRoutes,
                ClusterId = ClusterId.VideoLibrary,
                Match = new RouteMatch
                {
                    Path = "video-library/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        {"PathPattern", "{**catch-all}" }
                    }
                },
                AuthorizationPolicy = "Default"
            },
            // LEARNING SERVICE
            new RouteConfig
            {
                RouteId = RouteId.LearningService,
                ClusterId = ClusterId.LearningService,
                Match = new RouteMatch
                {
                    Path = "learning/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        {"PathPattern", "{**catch-all}" }
                    }
                },
                AuthorizationPolicy = "Default"
            },
            // WISH LIST
            new RouteConfig
            {
                RouteId = RouteId.WishListRoutes,
                ClusterId = ClusterId.WishList,
                Match = new RouteMatch
                {
                    Path = "wish-list/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        {"PathPattern", "{**catch-all}" }
                    }
                },
                AuthorizationPolicy = "Default"
            },
            // BASKET
            new RouteConfig
            {
                RouteId = RouteId.BasketRoutes,
                ClusterId = ClusterId.Basket,
                Match = new RouteMatch
                {
                    Path = "cart/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        {"PathPattern", "{**catch-all}" }
                    }
                },
                AuthorizationPolicy = "Default"
            },
            // ORDERING
            //new RouteConfig
            //{
            //    RouteId = RouteId.OrderingRoutes,
            //    ClusterId = ClusterId.Ordering,
            //    Match = new RouteMatch
            //    {
            //        Path = "ordering/{**catch-all}"
            //    },
            //    Transforms = new List<Dictionary<string, string>>
            //    {
            //        new Dictionary<string, string>
            //        {
            //            {"PathPattern", "{**catch-all}" }
            //        }
            //    },
            //    AuthorizationPolicy = "Default"
            //},
            // USER_SERVICE
            new RouteConfig
            {
                RouteId = RouteId.UserServiceRoutes,
                ClusterId = ClusterId.UserService,
                Match = new RouteMatch
                {
                    Path = "user/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        {"PathPattern", "{**catch-all}" }
                    }
                },
                AuthorizationPolicy = "Default"
            },
            // PAYMENT_SERVICE
            new RouteConfig
            {
                RouteId = RouteId.PaymentServiceRoutes,
                ClusterId = ClusterId.PaymentService,
                Match = new RouteMatch
                {
                    Path = "payment/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        {"PathPattern", "{**catch-all}" }
                    }
                }
            }
        };
        return routes.AsReadOnly();
    }
}