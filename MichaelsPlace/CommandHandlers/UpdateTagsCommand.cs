
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using JetBrains.Annotations;
using MediatR;
using MichaelsPlace.Controllers.Admin;
using MichaelsPlace.Infrastructure.Identity;
using MichaelsPlace.Models.Admin;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Utilities;
using Ninject;

namespace MichaelsPlace.CommandHandlers
{
    public class UpdateTagsCommand : IAsyncRequest<ICommandResult>
    {
        public ModelStateDictionary ModelState { get; set; }
        public List<AdminTagModel> Tags { get; set; }

        public UpdateTagsCommand([NotNull] List<AdminTagModel> tags, [NotNull] ModelStateDictionary modelState)
        {
            if (tags == null) throw new ArgumentNullException(nameof(tags));
            if (modelState == null) throw new ArgumentNullException(nameof(modelState));
            Tags = tags;
            ModelState = modelState;
        }
    }

    public class UpdateTagsCommandHandler : CommandHandlerBase, IAsyncRequestHandler<UpdateTagsCommand, ICommandResult>
    {
        public Task<ICommandResult> Handle(UpdateTagsCommand message)
        {
            var updates = message.Tags;
            var tags = DbContext.Tags.ToDictionary(t => t.Id);


            foreach (var adminTagModel in updates)
            {
                Tag existingTag;
                if (tags.TryGetValue(adminTagModel.Id, out existingTag))
                {
                    if (adminTagModel.State == EntityState.Deleted)
                    {
                        DbContext.Tags.Remove(existingTag);
                    }
                    else
                    {
                        Mapper.Map(adminTagModel, existingTag, typeof(AdminTagModel), existingTag.GetType());
                    }
                }
                else
                {
                    var tagType = adminTagModel.Type == AdminTagType.Relationship
                        ? typeof(RelationshipTag)
                        : adminTagModel.Type == AdminTagType.Loss
                            ? typeof(LossTag)
                            : typeof(ContextTag);

                    var tag = Mapper.Map(adminTagModel, typeof(AdminTagModel), tagType) as Tag;
                    DbContext.Tags.Add(tag);
                }
            }

            DbContext.SaveChanges();
            
            foreach (var adminTagModel in updates)
            {
                adminTagModel.State = EntityState.Unchanged;
            }

            return CommandResult.SuccessAsync();
        }
    }
}
