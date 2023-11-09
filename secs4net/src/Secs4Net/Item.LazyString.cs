﻿using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Secs4Net;

partial class Item
{
    [DebuggerTypeProxy(typeof(ItemDebugView))]
    [SkipLocalsInit]
    private sealed class LazyStringItem : Item
    {
        private readonly IMemoryOwner<byte> _owner;
        private readonly Lazy<string> _value;

        internal LazyStringItem(SecsFormat format, IMemoryOwner<byte> owner)
            : base(format)
        {
            _owner = owner;
            _value = new Lazy<string>(() =>
            {
                var encoding =  EncodingSetting.ASCIIEncoding;
                return encoding.GetString(_owner.Memory.Span);
            }, isThreadSafe: false);
        }

        public sealed override void Dispose()
            => _owner.Dispose();

        public sealed override int Count
            => _value.Value.Length;

        public sealed override string GetString()
            => _value.Value;

        public sealed override void EncodeTo(IBufferWriter<byte> buffer)
        {
            ReadOnlySpan<byte> bytes = _owner.Memory.Span;
            if (bytes.IsEmpty)
            {
                EncodeEmptyItem(Format, buffer);
                return;
            }
            EncodeItemHeader(Format, bytes.Length, buffer);
            buffer.Write(bytes);
        }

        private protected sealed override bool IsEquals(Item other)
            => Format == other.Format && _value.Value.Equals(other.GetString(), StringComparison.Ordinal);

        private sealed class ItemDebugView
        {
            private readonly LazyStringItem _item;
            public ItemDebugView(LazyStringItem item)
            {
                _item = item;
                EncodedBytes = new EncodedByteDebugView(item);
            }

            public string Value => _item._value.Value;
            public EncodedByteDebugView EncodedBytes { get; }
        }
    }
}
