using System;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Model.EditFields
{
    public abstract class AbstractField: IFieldEditable
    {
        private int pageNumber { get; set; }

        /// <inheritdoc />
        public int PageNumber
        {
            get { return pageNumber; }
            set
            {
                if (value < 0) { throw new ArgumentException("Value cannot be less than 0", nameof(PageNumber)); }

                pageNumber = value;
            }
        }

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
