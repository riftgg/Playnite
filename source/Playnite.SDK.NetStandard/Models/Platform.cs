﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playnite.SDK.Models
{
    /// <summary>
    /// Represents game's platfom.
    /// </summary>
    public class Platform : DatabaseObject
    {
        private string icon;
        /// <summary>
        /// Gets or sets platform icon.
        /// </summary>
        public string Icon
        {
            get => icon;
            set
            {
                icon = value;
                OnPropertyChanged();
            }
        }

        private string cover;
        /// <summary>
        /// Gets or sets default game cover.
        /// </summary>
        public string Cover
        {
            get => cover;
            set
            {
                cover = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Creates new instance of Platform.
        /// </summary>
        public Platform() : base()
        {
        }

        /// <summary>
        /// Creates new instance of Platform with specific name.
        /// </summary>
        /// <param name="name">Platform name.</param>
        public Platform(string name) : this()
        {
            Name = name;
        }

        /// <inheritdoc/>
        public override string ToString()
        {            
            return Name;
        }

        /// <summary>
        /// Gets empty platform.
        /// </summary>
        public static readonly Platform Empty = new Platform { Id = Guid.Empty, Name = string.Empty };
    }
}
