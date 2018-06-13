using System;
using System.IO;

namespace Yu_Gi_Oh.Save_File
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     This class is only inherted. Don't use it.
    /// </summary>
    /// <remarks>
    ///     Johnathon, 6/06/2018.
    /// </remarks>
    /// -------------------------------------------------------------------------------------------------
    [Obsolete("This class is only inherted. Don\'t use it.")]
    public class Save_Data_Chunk
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Does Nothing. Don't Use.
        /// </summary>
        /// <remarks>
        ///     Johnathon, 6/06/2018.
        /// </remarks>
        /// -------------------------------------------------------------------------------------------------
        public virtual void Clear()
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Does Nothing. Don't Use.
        /// </summary>
        /// <remarks>
        ///     Johnathon, 6/06/2018.
        /// </remarks>
        /// <param name="Buffer">
        ///     Does Nothing. Don't Use.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public virtual void Load(byte[] Buffer)
        {
            using (var Reader = new BinaryReader(new MemoryStream(Buffer)))
            {
                Load(Reader);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Does Nothing. Don't Use.
        /// </summary>
        /// <remarks>
        ///     Johnathon, 6/06/2018.
        /// </remarks>
        /// <param name="Reader">
        ///     Does Nothing. Don't Use.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public virtual void Load(BinaryReader Reader)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Does Nothing. Don't Use.
        /// </summary>
        /// <remarks>
        ///     Johnathon, 6/06/2018.
        /// </remarks>
        /// <returns>
        ///     A byte[].
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public virtual byte[] Save()
        {
            using (var Stream = new MemoryStream())
            using (var Writer = new BinaryWriter(Stream))
            {
                Save(Writer);
                return Stream.ToArray();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Does Nothing. Don't Use.
        /// </summary>
        /// <remarks>
        ///     Johnathon, 6/06/2018.
        /// </remarks>
        /// <param name="Writer">
        ///     Does Nothing. Don't Use.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public virtual void Save(BinaryWriter Writer)
        {
        }
    }
}