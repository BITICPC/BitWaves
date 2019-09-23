namespace BitWaves.Data
{
    /// <summary>
    /// 提供字节缓冲区相关的方法。
    /// </summary>
    internal static class BufferUtils
    {
        /// <summary>
        /// 逐字节比较缓冲区内容是否相同。
        /// </summary>
        /// <param name="p1">指向第一个缓冲区的指针。</param>
        /// <param name="p2">指向第二个缓冲区的指针。</param>
        /// <param name="size">两个缓冲区的字节大小。</param>
        /// <returns>两个缓冲区中的内容是否相同。</returns>
        private static unsafe bool NaiveEquals(byte* p1, byte* p2, int size)
        {
            while (size > 0)
            {
                if (*p1++ != *p2++)
                {
                    return false;
                }

                size--;
            }

            return true;
        }

        /// <summary>
        /// 比较两个缓冲区内容是否相同。
        /// </summary>
        /// <param name="p1">指向第一个缓冲区的指针。调用者应确保该指针已经对齐到 8 字节边界。</param>
        /// <param name="p2">指向第二个缓冲区的指针。调用者应确保该指针已经对齐到 8 字节边界。</param>
        /// <param name="size">缓冲区的字节大小。</param>
        /// <returns>两个缓冲区中的内容是否相同。</returns>
        private static unsafe bool EqualsAligned(byte* p1, byte* p2, int size)
        {
            while (size >= sizeof(long))
            {
                if (*(long*) p1 != *(long*) p2)
                {
                    return false;
                }

                p1 += sizeof(long);
                p2 += sizeof(long);
                size -= sizeof(long);
            }

            if (size >= sizeof(int))
            {
                if (*(int*) p1 != *(int*) p2)
                {
                    return false;
                }

                p1 += sizeof(int);
                p2 += sizeof(int);
                size -= sizeof(int);
            }

            if (size >= sizeof(short))
            {
                if (*(short*) p1 != *(short*) p2)
                {
                    return false;
                }

                p1 += sizeof(short);
                p2 += sizeof(short);
                size -= sizeof(short);
            }

            if (size >= sizeof(byte))
            {
                if (* p1 != * p2)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 逐字节比较两缓冲区中的内容，直到到达缓冲区边界或者缓冲区指针对齐到 8 字节边界为止。
        /// </summary>
        /// <param name="p1">指向第一个缓冲区的指针。</param>
        /// <param name="p2">指向第二个缓冲区的指针。</param>
        /// <param name="size">缓冲区的字节大小。</param>
        /// <returns>两缓冲区中的内容是否相同。</returns>
        private static unsafe bool EqualsUntilAlignment(ref byte* p1, ref byte* p2, ref int size)
        {
            // 检查 p1 与 p2 是否能够同时对齐到 8 字节边界
            if ((((ulong) p1) & 7) != (((ulong) p2) & 7))
            {
                // 无法同时对其到 8 字节边界
                var result = NaiveEquals(p1, p2, size);
                p1 += size;
                p2 += size;
                size = 0;
                return result;
            }

            var movement = (int) ((ulong) p1 & 7);
            if (movement > size)
            {
                // 缓冲区大小不足以支持指针移动到 8 字节边界
                var result = NaiveEquals(p1, p2, size);
                p1 += size;
                p2 += size;
                size = 0;
                return result;
            }

            // 首先逐字节比较到 8 字节边界
            if (!NaiveEquals(p1, p2, movement))
            {
                return false;
            }

            p1 += movement;
            p2 += movement;
            size -= movement;

            return true;
        }

        /// <summary>
        /// 检查两个给定缓冲区中的内容是否相同。
        /// </summary>
        /// <param name="buffer1">第一个缓冲区。</param>
        /// <param name="buffer2">第二个缓冲区。</param>
        /// <returns>两个缓冲区中的内容是否相同。</returns>
        public static bool Equals(byte[] buffer1, byte[] buffer2)
        {
            if (ReferenceEquals(buffer1, buffer2))
            {
                return true;
            }

            if (buffer1 == null || buffer2 == null)
            {
                return false;
            }

            if (buffer1.Length != buffer2.Length)
            {
                return false;
            }

            unsafe
            {
                fixed (byte* pBuffer1 = buffer1)
                fixed (byte* pBuffer2 = buffer2)
                {
                    var p1 = pBuffer1;
                    var p2 = pBuffer2;
                    var size = buffer1.Length;

                    // 逐字节比较直到对齐到 8 字节边界
                    if (!EqualsUntilAlignment(ref p1, ref p2, ref size))
                    {
                        return false;
                    }

                    return EqualsAligned(p1, p2, size);
                }
            }
        }
    }
}
