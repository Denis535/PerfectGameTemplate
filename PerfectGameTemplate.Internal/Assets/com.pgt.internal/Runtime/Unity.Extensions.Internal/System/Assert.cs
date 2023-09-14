﻿#nullable enable
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;

    public static class Assert {
        public static class Argument {
            public static Assertions.Message<Assertions.Argument> Message(FormattableString text) => new( text.GetDisplayString() );
        }
        public static class Operation {
            public static Assertions.Message<Assertions.Operation> Message(FormattableString text) => new( text.GetDisplayString() );
        }
        public static class Object {
            public static Assertions.Message<Assertions.Object> Message(FormattableString text) => new( text.GetDisplayString() );
        }
        public static class Internal {
            public static Assertions.Message<Assertions.Internal> Message(FormattableString text) => new( text.GetDisplayString() );
        }
    }
    // Assertions
    public static class Assertions {
        public class Message<T> {
            internal string Value { get; }
            public Message(string value) {
                Value = value;
            }
            public override string ToString() {
                return Value;
            }
        }
        public abstract class Argument {
        }
        public abstract class Operation {
        }
        public abstract class Object {
        }
        public abstract class Internal {
        }

        // Argument
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void Valid(this Message<Argument> message, [DoesNotReturnIf( false )] bool isValid) {
            if (!isValid) throw Exceptions.Exception<ArgumentException>( message.Value );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void InRange(this Message<Argument> message, [DoesNotReturnIf( false )] bool isValid) {
            if (!isValid) throw Exceptions.Exception<ArgumentOutOfRangeException>( message.Value );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void NotNull(this Message<Argument> message, [DoesNotReturnIf( false )] bool isValid) {
            if (!isValid) throw Exceptions.Exception<ArgumentNullException>( message.Value );
        }

        // Operation
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void Valid(this Message<Operation> message, [DoesNotReturnIf( false )] bool isValid) {
            if (!isValid) throw Exceptions.Exception<InvalidOperationException>( message.Value );
        }

        // Object
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void Valid(this Message<Object> message, [DoesNotReturnIf( false )] bool isValid) {
            if (!isValid) throw Exceptions.Exception<ObjectInvalidException>( message.Value );
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void Alive(this Message<Object> message, [DoesNotReturnIf( false )] bool isValid) {
            if (!isValid) throw Exceptions.Exception<ObjectDisposedException>( message.Value );
        }

    }
}
