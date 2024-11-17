using QDApps.Models.WhereItAppModels.ViewModels;
using QDApps.Models.WhereItAppModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;


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

        public ViewInventory GetInventory(int userId)
        {
            ViewInventory inventory = new()
            {
                Stashes = GetStashes(userId),
                Items = GetItems(userId),
                Tags = GetTags(userId),
                UserName = _context.Users.Single(x => x.UserId == userId).UserName

            };            
            return inventory;
        }
        public List<ViewStashes> GetStashes(int userId)
        {
            List<ViewStashes> stashes = _context.ViewStashes.Where(x => x.UserId == userId)
                                                            .Select( x => new ViewStashes()
                                                            {
                                                                UserId = x.UserId,
                                                                StashName = x.StashName,
                                                                StashId = x.StashId,
                                                                ItemCount = x.ItemCount
                                                                
                                                            }).ToList();

            foreach (var stash in stashes)
            {
                stash.Tags = _context.StashTags.Where(x => x.StashId == stash.StashId).Select(x => new Tag()
                {
                    TagId = x.TagId,
                    TagName = x.TagName
                }).ToList();
            }

            return stashes;
        }
        public List<ViewItems> GetItems(int userId)
        {
            List<ViewItems> items = _context.ViewItems.Where(x => x.UserId == userId)
                                                      .Select(x => new ViewItems()
                                                      {
                                                          UserId = x.UserId,
                                                          ItemId = x.ItemId,
                                                          ItemName = x.ItemName,
                                                          StashId = x.StashId,
                                                          StashName = x.StashName
                                                      })
                                                      .ToList();
            foreach(var item in items)
            {
                item.Tags = _context.ItemTagNames.Where(x => x.ItemId == item.ItemId)
                                                     .Select(x => new Tag()
                                                     {
                                                         TagId= x.TagId,
                                                         TagName = x.TagName
                                                     }).ToList();
            }

            return items;
        }
        public List<ViewItems> GetItems(int userId, int stashId)
        {
            List<ViewItems> items = _context.ViewItems.Where(x => x.UserId == userId
                                                               && x.StashId == stashId)
                                                      .Select(x => new ViewItems()
                                                      {
                                                          UserId = x.UserId,
                                                          ItemId = x.ItemId,
                                                          ItemName = x.ItemName,
                                                          StashId = x.StashId,
                                                          StashName = x.StashName
                                                      })
                                                      .ToList();
            foreach (var item in items)
            {
                item.Tags = _context.ItemTagNames.Where(x => x.ItemId == item.ItemId)
                                                     .Select(x => new Tag()
                                                     {
                                                         TagId = x.TagId,
                                                         TagName = x.TagName
                                                     }).ToList();
            }

            return items;
        }

        public ViewItem GetItem(int userId, int itemId)
        {
            Item item = _context.Items.Single(x => x.ItemId == itemId);

            List<Tag> tags = _context.ItemTagNames.Where(x => x.ItemId == item.ItemId)
                                                     .Select(x => new Tag()
                                                     {
                                                         TagId = x.TagId,
                                                         TagName = x.TagName
                                                     }).ToList();
            List<Stash> availableStashes = _context.Stashes.Where(x => x.UserId == userId).ToList();

            List<Tag> availableTags = _context.Tags.Where(x => x.UserId == userId
                                                            && !tags.Select(y => y.TagId).Contains(x.TagId)).ToList();

            ViewItem viewItem = new()
            {
                ItemId = item.ItemId,
                StashId = item.StashId,
                ItemName = item.ItemName,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                Tags = tags,
                AvailableStashes = availableStashes,
                AvailableTags = availableTags
            };

            return viewItem;
        }
        public List<ViewTags> GetTags(int userId)
        {
            List<ViewTags> tags = _context.ViewTags.Where(x => x.UserId == userId).ToList();
            return tags;
        }
        public ViewTag GetTag(int userId, int tagId)
        {
            Tag tag = _context.Tags.Single(x => x.UserId == userId
                                             && x.TagId == tagId);
            List<ViewItems> viewItems = _context.ItemTagNames.Where(x => x.TagId == tagId
                                                                            && x.UserId == userId)
                                                             .Select(x => new ViewItems()
                                                             {
                                                                 ItemId = x.ItemId,
                                                                 ItemName = x.ItemName,
                                                                 StashName = x.StashName
                                                             })
                                                             .ToList();
            ViewTag viewTag = new()
            {
                TagId = tagId,
                TagName = tag.TagName,
                CreatedAt = tag.CreatedAt,
                UpdatedAt = tag.UpdatedAt,
                EditedTagName = tag.TagName,
                UserId = userId,
                ViewItems = viewItems
            };
            return viewTag;
        }
        public ViewStash GetStash(int userId, int stashId)
        {
            Stash stash = _context.Stashes.Single(x => x.StashId == stashId
                                                    && x.UserId == userId);

            List<ViewItems> viewItems = GetItems(userId, stashId);

            List<Stash> availableStashes = _context.Stashes.Where(x => x.UserId == userId
                                                                    && x.StashId != stashId).ToList();

            availableStashes.Add(new Stash()
            {
                StashId = 0,
                StashName = "Move/Delete Selected..."
            });
            availableStashes.Add(new Stash()
            {
                StashId = -1,
                StashName = "Delete Items"
            });


            ViewStash viewStash = new()
            {
                StashId = stash.StashId,
                StashName = stash.StashName,
                EditedStashName = stash.StashName,
                UserId = stash.UserId,
                CreatedAt = stash.CreatedAt,
                UpdatedAt = stash.UpdatedAt,
                ViewItems = viewItems,
                AvailableStashes = availableStashes.OrderBy(x => x.StashId).ToList()
            };


            return viewStash;
        }
        public Status AddItemTag(int tagId, int itemId)
        {
            Status status = new();

            //duplicate check
            if(_context.ItemTags.Any(x => x.ItemId == itemId && x.TagId == tagId))
            {
                status.IsSuccess = true;
                return status;
            };

            try
            {
                _context.ItemTags.Add(new ItemTag()
                {
                    ItemId = itemId,
                    TagId = tagId
                });
                Item item = _context.Items.Single(x => x.ItemId == itemId);
                item.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
                status.IsSuccess = true;

            }
            catch
            {
                status.IsSuccess = false;
                status.StatusMessage = "Well dang it, failed to add tag to item...";
            }

            return status;
        }
        public Status RemoveItemTag(int tagId, int itemId)
        {   
            Status status = new();

            if (_context.ItemTags.Any(x => x.ItemId == itemId && x.TagId == tagId))
            {
                try
                {
                    ItemTag itemTag = _context.ItemTags.Single(x => x.TagId == tagId && x.ItemId == itemId);
                    _context.ItemTags.Remove(itemTag);
                    Item item = _context.Items.Single(x => x.ItemId == itemId);
                    item.UpdatedAt = DateTime.UtcNow;
                    _context.SaveChanges();
                    status.IsSuccess = true;
                }
                catch
                {
                    status.IsSuccess = false;
                    status.StatusMessage = "For the record: failed to remove record of tag on item...";
                }
            }; 
            return status;
        }
        public Status EditItem(ViewItem item)
        {
            Status status = new();

            Item editedItem = _context.Items.Single(x => x.ItemId == item.ItemId);
            editedItem.ItemName = item.ItemName;
            editedItem.StashId = item.StashId;
            editedItem.UpdatedAt = DateTime.UtcNow;

            try
            {
                _context.Items.Update(editedItem);
                _context.SaveChanges();
                status.IsSuccess = true;
            }
            catch
            {
                status.IsSuccess = false;
                status.StatusMessage = "The ball was dropped and we failed saving changes to your item";
            }

            return status;
        }
        public Status EditStash(int userId, ViewStash stash)
        {
            Status status = new();

            // delete items
            if (stash.DestinationStashId == -1)
            {
                List<Item> itemsToRemove = new();
                foreach (var item in stash.ViewItems)
                {
                    if (item.Selected)
                    {
                        itemsToRemove.Add(_context.Items.Single(x=> x.ItemId == item.ItemId));
                    }
                }

                Stash stashToEdit = _context.Stashes.Single(x => x.StashId == stash.StashId);
                stashToEdit.UpdatedAt = DateTime.UtcNow;
                try
                {
                    _context.Items.RemoveRange(itemsToRemove);
                    _context.Stashes.Update(stashToEdit);
                    _context.SaveChanges();
                    status.IsSuccess = true;
                }
                catch
                {
                    status.IsSuccess = false;
                    status.StatusMessage = "I've failed in so many things. Including, just now, removing these items from this stash. But also lots of other stuff, I assure you. Oh woe!";
                    return status;
                }
            }
            // move items
            if (stash.DestinationStashId > 0)
            {
                List<Item> itemsToMove = new();
                foreach (var item in stash.ViewItems)
                {
                    if (item.Selected)
                    {
                        itemsToMove.Add(_context.Items.Single(x => x.ItemId == item.ItemId));
                    }
                }
                itemsToMove.ForEach(x => x.StashId = stash.DestinationStashId);
                itemsToMove.ForEach(x => x.UpdatedAt = DateTime.UtcNow);

                Stash stashToEdit = _context.Stashes.Single(x => x.StashId == stash.StashId);
                stashToEdit.UpdatedAt = DateTime.UtcNow;
                try
                {
                    _context.Items.UpdateRange(itemsToMove);
                    _context.Stashes.Update(stashToEdit);
                    _context.SaveChanges();
                    status.IsSuccess = true;
                }
                catch
                {
                    status.IsSuccess = false;
                    status.StatusMessage = "Dread and woe, dread and woe. I failed to move these items, you know!";
                    return status;
                }

            }
            // edit name
            if(stash.EditedStashName != stash.StashName)
            {
                Stash stashToEdit = _context.Stashes.Single(x => x.StashId == stash.StashId);
                stashToEdit.UpdatedAt = DateTime.UtcNow;
                stashToEdit.StashName = stash.EditedStashName;
                try
                {
                    _context.Stashes.Update(stashToEdit);
                    _context.SaveChanges();
                    status.IsSuccess = true;
                }
                catch
                {
                    status.IsSuccess = false;
                    status.StatusMessage = "Splish Splash, failed to update your stash!";
                    return status;
                }

            }



            return status;
        }
        public Status EditTag(int userId, ViewTag tag)
        {
            Status status = new();
            Tag editedTag = _context.Tags.Single(x => x.TagId == tag.TagId);
            editedTag.UpdatedAt = DateTime.UtcNow;
            editedTag.TagName = tag.EditedTagName;

            try
            {
                _context.Tags.Update(editedTag);
                _context.SaveChanges();
                status.IsSuccess = true;
            }
            catch
            {
                status.IsSuccess = false;
                status.StatusMessage = "Tag, that's it. Failed to edit tag.";
                return status;
            }
            return status;
        }

        public bool IsItemOwnedByUser(int userId, int itemId)
        {
            return _context.ViewItems.Any(x => x.ItemId == itemId && x.UserId == userId);
        }
        public bool IsTagOwnedByUser(int userId, int tagId)
        {
            return _context.Tags.Any(x => x.TagId == tagId && x.UserId == userId);
        }
        public bool IsStashOwnedByUser(int userId, int stashId)
        {
            return _context.Stashes.Any(x => x.StashId == stashId && x.UserId == userId);
        }
    }
}
