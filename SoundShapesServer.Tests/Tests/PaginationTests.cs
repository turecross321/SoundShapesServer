using SoundShapesServer.Extensions;
using SoundShapesServer.Tests.Server;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Tests.Tests;

public class PaginationTests : ServerTest
{
    [Test]
    public void TakeWorks()
    {
        using SSTestContext context = this.GetServer();
        const int itemsToCreate = 150;
        
        for (int i = 0; i < itemsToCreate; i++)
        {
            context.CreateUser();
        }

        IQueryable<DbUser> users = context.Database.GetUsers();
        
        PaginatedDbList<DbUser, Guid> paginated = users.PaginateWithGuidId(new PageData
        {
            Skip = 0,
            Take = itemsToCreate,
            MinimumCreationDate = null,
            MaximumCreationDate = null,
            ExcludeIds = []
        });
        
        // Limit works
        Assert.That(paginated.Items.Count(), Is.EqualTo(PaginatedDbList<DbUser, Guid>.MaxItems));

        paginated = users.PaginateWithGuidId(new PageData
        {
            Skip = 0,
            Take = 1,
            MinimumCreationDate = null,
            MaximumCreationDate = null,
            ExcludeIds = []
        });
        
        Assert.Multiple(() =>
        {
            // It takes the right amount of items
            Assert.That(paginated.Items.Count(), Is.EqualTo(1));
            // TotalItems works correctly
            Assert.That(paginated.TotalItems, Is.EqualTo(itemsToCreate));
        });
    }

    [Test]
    public void SkipWorks()
    {
        using SSTestContext context = this.GetServer();

        DbUser firstUser = context.CreateUser();
        const int itemsToCreate = 10;
        
        for (int i = 0; i < itemsToCreate; i++)
        {
            context.CreateUser();
        }
        
        IQueryable<DbUser> users = context.Database.GetUsers();
        
        PaginatedDbList<DbUser, Guid> paginated = users.PaginateWithGuidId(new PageData
        {
            Skip = 1,
            Take = itemsToCreate,
            MinimumCreationDate = null,
            MaximumCreationDate = null,
            ExcludeIds = []
        });
        
        Assert.That(paginated.Items.First().Id, Is.Not.EqualTo(firstUser.Id));
    }

    [Test]
    public void ExcludeIdsWorks()
    {
        using SSTestContext context = this.GetServer();

        DbUser firstUser = context.CreateUser();
        const int itemsToCreate = 10;
        
        for (int i = 0; i < itemsToCreate; i++)
        {
            context.CreateUser();
        }
        
        IQueryable<DbUser> users = context.Database.GetUsers();
        
        PaginatedDbList<DbUser, Guid> paginated = users.PaginateWithGuidId(new PageData
        {
            Skip = 0,
            Take = itemsToCreate,
            MinimumCreationDate = null,
            MaximumCreationDate = null,
            ExcludeIds = [firstUser.Id.ToString()]
        });
        
        Assert.That(paginated.Items.First().Id, Is.Not.EqualTo(firstUser.Id));
    }

    [Test]
    public void DateFilterWorks()
    {
        using SSTestContext context = this.GetServer();

        const int oldItemAmount = 5;
        const int newItemAmount = 10;

        DateTime oldDate = context.Time.Now;
        for (int i = 0; i < oldItemAmount; i++)
        {
            context.CreateUser();
        }

        context.Time.Now = context.Time.Now.AddYears(1);
        DateTime newDate = context.Time.Now;
        for (int i = 0; i < newItemAmount; i++)
        {
            context.CreateUser();
        }
        
        IQueryable<DbUser> users = context.Database.GetUsers();
        
        PaginatedDbList<DbUser, Guid> paginated = users.PaginateWithGuidId(new PageData
        {
            Skip = 0,
            Take = 100,
            MinimumCreationDate = newDate,
            MaximumCreationDate = null,
            ExcludeIds = []
        });
        
        Assert.That(paginated.TotalItems, Is.EqualTo(newItemAmount));
        foreach (DbUser user in paginated.Items)
        {
            Assert.That(user.CreationDate, Is.EqualTo(newDate));
        }
        
        paginated = users.PaginateWithGuidId(new PageData
        {
            Skip = 0,
            Take = 100,
            MinimumCreationDate = null,
            MaximumCreationDate = oldDate,
            ExcludeIds = []
        });
        
        Assert.That(paginated.TotalItems, Is.EqualTo(oldItemAmount));
        foreach (DbUser user in paginated.Items)
        {
            Assert.That(user.CreationDate, Is.EqualTo(oldDate));
        }
    }
}