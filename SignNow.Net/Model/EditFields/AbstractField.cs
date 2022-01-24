using System;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Model.EditFields
{
    public abstract class AbstractField: IFieldEditable
    {
        /// <inheritdoc />
        public int PageNumber { get; set; }

        /// <inheritdoc />
        public virtual string Type => String.Empty;

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string Role { get; set; }

        /// <inheritdoc />
        public bool Required { get; set; }

        /// <inheritdoc />
        public int X { get; set; }

        /// <inheritdoc />
        public int Y { get; set; }

        /// <inheritdoc />
        public int Width { get; set; }

        /// <inheritdoc />
        public int Height { get; set; }
    }
}
