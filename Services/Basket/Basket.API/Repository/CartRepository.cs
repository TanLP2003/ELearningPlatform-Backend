using Basket.API.Infrastructure;
using Basket.API.Models;
using EventBus.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Basket.API.Repository;

public class CartRepository(CartDbContext context) : ICartRepository
{
    //private readonly IMongoClient mongoDbClient;
    //private readonly IMongoCollection<Cart> cartCollection;
    //private readonly IMongoCollection<OutboxMessage> outboxCollection;
    //private readonly ILogger<CartRepository> logger;

    //public CartRepository(IOptions<MongoDbSetting> settings)
    //{
    //    mongoDbClient = new MongoClient(settings.Value.ConnectionString);
    //    var mongoDatabase = mongoDbClient.GetDatabase(settings.Value.DatabaseName);
    //    cartCollection = mongoDatabase.GetCollection<Cart>(settings.Value.CollectionName);
    //    outboxCollection = mongoDatabase.GetCollection<OutboxMessage>("OutboxMessage");
    //}

    //public async Task<Cart> GetBasket(Guid userId)
    //{
    //    var projection = Builders<Cart>.Projection.Exclude("_id");
    //    return await cartCollection.Find(c => c.UserId == userId)
    //                               .Project<Cart>(projection)
    //                               .FirstOrDefaultAsync();
    //}
    //public async Task CreateCart(Cart cart)
    //{
    //    await cartCollection.InsertOneAsync(cart);
    //}
    //public async Task<Cart> AddCartItem(Guid userId, CartItem item)
    //{
    //    var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
    //    var update = Builders<Cart>.Update.Push(c => c.Items, item);
    //    var projection = Builders<Cart>.Projection.Exclude("_id");
    //    var options = new FindOneAndUpdateOptions<Cart>
    //    {
    //        IsUpsert = true,
    //        ReturnDocument = ReturnDocument.After,
    //        Projection = projection,
    //    };

    //    var result = await cartCollection.FindOneAndUpdateAsync(filter, update, options);
    //    return result;
    //}

    //public async Task<Cart> RemoveCartItem(Guid userId, CartItem item)
    //{
    //    var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId)
    //        & Builders<Cart>.Filter.Where(c => c.Items.Any(i => i.CourseId == item.CourseId));
    //    var update = Builders<Cart>.Update.PullFilter(c => c.Items, ci => ci.CourseId == item.CourseId);
    //    var projection = Builders<Cart>.Projection.Exclude("_id");
    //    var options = new FindOneAndUpdateOptions<Cart>
    //    {
    //        ReturnDocument = ReturnDocument.After,
    //        Projection = projection
    //    };

    //    var result = await cartCollection.FindOneAndUpdateAsync(filter, update, options);
    //    return result;
    //}
    //public async Task<Cart> CheckoutBasket(Guid userId, OutboxMessage outBoxMessage)
    //{
    //    var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
    //    var update = Builders<Cart>.Update.Set(c => c.Items, new List<CartItem>());
    //    var projection = Builders<Cart>.Projection.Exclude("_id");
    //    var options = new FindOneAndUpdateOptions<Cart>
    //    {
    //        ReturnDocument = ReturnDocument.After,
    //        Projection = projection
    //    };
    //    using (var session = mongoDbClient.StartSession())
    //    {
    //        session.StartTransaction();
    //        try
    //        {
    //            var result = await cartCollection.FindOneAndUpdateAsync(session, filter, update, options);
    //            await outboxCollection.InsertOneAsync(session, outBoxMessage);
    //        }
    //        catch (Exception ex)
    //        {
    //            await session.AbortTransactionAsync();
    //            logger.LogError(ex.Message);
    //        }
    //    }
    //    return await GetBasket(userId);
    //}

    //public async Task<Cart> CleanBasket(Guid userId)
    //{
    //    var filter = Builders<Cart>.Filter.Eq(c => c.UserId, userId);
    //    var update = Builders<Cart>.Update.Set(c => c.Items, new List<CartItem>());
    //    var projection = Builders<Cart>.Projection.Exclude("_id");
    //    var options = new FindOneAndUpdateOptions<Cart>
    //    {
    //        ReturnDocument = ReturnDocument.After,
    //        Projection = projection
    //    };
    //    var result = await cartCollection.FindOneAndUpdateAsync(filter, update, options);
    //    return result;
    //}
    public async Task<Cart> GetBasket(Guid userId)
    {
        var cart = await context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        if(cart == null)
        {
            cart = new Cart(userId);
            await context.Carts.AddAsync(cart);
            await context.SaveChangesAsync();
        }
        return cart;
    }
    public async Task<Cart> AddCartItem(Guid userId, CartItem item)
    {
        var cart = await context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        cart.Items.Add(item);
        return cart;
    }

    public async Task<Cart> CleanBasket(Guid userId)
    {
        var cart = await context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        cart.Items = new();
        return cart;
    }

    public async Task AddOutboxMessage(OutboxMessage outboxMessage)
    {
        await context.OutboxMessages.AddAsync(outboxMessage);
    }

    public async Task<OutboxMessage> GetOutboxMessageByEventId(Guid outboxEventId)
    {
        var outboxEvent = await context.OutboxMessages.FirstOrDefaultAsync(e => e.EventId == outboxEventId);
        return outboxEvent;
    }

    public async Task<Cart> RemoveCartItem(Guid userId, CartItem item)
    {
        var cart = await context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        cart.Items.Remove(item);
        return cart;
    }
    public async Task CommitAsync()
    {
        await context.SaveChangesAsync();
    }
}