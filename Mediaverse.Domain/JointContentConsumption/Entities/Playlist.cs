using System;
using System.Linq;
using System.Collections.Generic;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;

namespace Mediaverse.Domain.JointContentConsumption.Entities
{
    public class Playlist : Entity
    {
        private readonly IList<Content> _contents;
        
        public Host Owner { get; }
        public bool IsTemporary => Owner == null;
        
        public int Count => _contents.Count;
        
        public Playlist(Guid id, IEnumerable<Content> contents = null, Host owner = null) : base(id)
        {
            try
            {
                _contents = contents?.ToList() ?? new List<Content>();
                Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create playlist", exception);
            }
        }

        public IEnumerator<Content> GetEnumerator() => _contents.GetEnumerator();

        public void Add(Content content)
        {
            try
            {
                _ = content ?? throw new ArgumentNullException(nameof(content));

                if (Contains(content))
                {
                    throw new InvalidOperationException("Content is added already");
                }
                
                _contents.Add(content);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not add content {content} to playlist {this}", exception);
            }
        }
        
        public void Remove(Content content)
        {
            try
            {
                _ = content ?? throw new ArgumentNullException(nameof(content));

                if (!Contains(content))
                {
                    throw new InvalidOperationException("Playlist does not contain specified item");
                }
                
                if (!_contents.Remove(content))
                {
                    throw new InvalidOperationException("Something went wrong");
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Could not remove content {content} from playlist {this}", exception);
            }
        }

        public bool Contains(Content content) => _contents.Contains(content);

        public Content this[int index] => _contents[index];
    }
}