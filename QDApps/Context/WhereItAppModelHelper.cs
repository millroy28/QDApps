using QDApps.Models.WhereItAppModels.ViewModels;
using QDApps.Models.WhereItAppModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace QDApps.Context
{
    public class WhereItAppModelHelper : ModelHelper
    {
        public WhereItAppModelHelper(QdappsContext context) : base(context)
        {
        }

        public bool IsUserNew(int userId)
        {
            bool isNew = !_context.Stashes.Where(x=>x.UserId == userId).Any();
            return isNew;
        }
        public Status CreateWelcomeStashSet(EditWelcomeStash editWelcomeStash)
        {
            Status status = new();
            status = CreateStash(editWelcomeStash.UserId, editWelcomeStash.Stash);
            if (status.IsSuccess) { status = CreateItem(status.RecordId, editWelcomeStash.Item); };
            if (status.IsSuccess) { status = CreateTag(editWelcomeStash.UserId, status.RecordId, editWelcomeStash.Tag); };
            return status;
        }

        public Status CreateStash(int userId, string stash)
        {
            Status status = new();
            try
            {
                Stash newStash = new()
                {
                    StashName = stash,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Stashes.Add(newStash);
                _context.SaveChanges();
                status.IsSuccess = true;
                status.RecordId = newStash.StashId;
            }
            catch
            {
                status.IsSuccess = false;
                status.StatusMessage = "Failed to save Stash!";
            }
            return status;
        }

        public Status CreateItem(int stashId, string item)
        {
            Status status = new();
            try
            {
                Item newItem = new()
                {
                    StashId = stashId,
                    ItemName = item,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Items.Add(newItem);
                _context.SaveChanges();
                status.IsSuccess = true;
                status.RecordId = newItem.ItemId;
            }
            catch
            {
                status.IsSuccess = false;
                status.StatusMessage = "Failed to save Item!";
            }
            return status;
        }

        public Status CreateTag(int userId, int itemId, string tag)
        {
            Status status = new();
            if (!_context.Items.Any(x => x.ItemId == itemId))
            {
                status.IsSuccess = false;
                status.StatusMessage = "Create Tag failed - Item does not exist - whaaa?!";
                return status;
            }
            if (!_context.Users.Any(x => x.UserId == userId)) 
            {
                status.IsSuccess = false;
                status.StatusMessage = "Create Tag failed - User with this id doesn't exist - whatteryoudoin?";
                return status;
            }

            try
            {
                Tag newTag = new()
                {
                    UserId = userId,
                    TagName = tag,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow                    
                };
                _context.Tags.Add(newTag);
                _context.SaveChanges();

                status = CreateItemTag(itemId, newTag.TagId);
                if (status.IsSuccess)
                {
                    status.RecordId = newTag.TagId;
                };

                return status;
            }
            catch
            {
                status.IsSuccess = false;
                status.StatusMessage = "Create Tag failed - the item existed and the user existed i don't know what happened it was all going so well i thought we could do this :( ";
                return status;
            }
        }
        public Status CreateTag(int userId, string tag)
        {
            Status status = new();
            if (!_context.Users.Any(x => x.UserId == userId))
            {
                status.IsSuccess = false;
                status.StatusMessage = "Create Tag failed - User with this id doesn't exist - whatteryoudoin?";
                return status;
            }

            try
            {
                Tag newTag = new()
                {
                    UserId = userId,
                    TagName = tag,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Tags.Add(newTag);
                _context.SaveChanges();

                status.RecordId = newTag.TagId;
                return status;
            }
            catch
            {
                status.IsSuccess = false;
                status.StatusMessage = "Create Tag failed - TEAM stands for Together wE Achieve More but I guess we didn't all try hard enough this time. And you are the victim. We are really sorry. Probably.";
                return status;
            }
        }
        public Status CreateItemTag(int itemId, int tagId) 
        {
            Status status = new();
            
            if (!_context.Items.Any(x => x.ItemId == itemId))
            {
                status.IsSuccess = false;
                status.StatusMessage = "Can't link tag to item that doesn't exist y'all";
                return status;
            }
            if (!_context.Tags.Any(x => x.TagId == tagId))
            {
                status.IsSuccess = false;
                status.StatusMessage = "Can't link item to a tag that doesn't exist brah";
                return status;
            }
            if (_context.ItemTags.Any(x => x.TagId == tagId && x.ItemId == itemId))
            { // we ain't a duplicate itemtag factory here but we'll let you try anyway
                status.IsSuccess = true;
                return status;
            }

            try
            {
                ItemTag itemTag = new()
                {
                    ItemId = itemId,
                    TagId = tagId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.ItemTags.Add(itemTag);
                _context.SaveChanges();

                status.IsSuccess = true;
                status.RecordId = itemTag.ItemTagId;
                return status;
            } 
            catch
            {
                status.IsSuccess = false;
                status.StatusMessage = "Linking Item and Tag just didn't happen right now. Maybe it wasn't meant to be.";
                return status;
            }
        }

    }
}
