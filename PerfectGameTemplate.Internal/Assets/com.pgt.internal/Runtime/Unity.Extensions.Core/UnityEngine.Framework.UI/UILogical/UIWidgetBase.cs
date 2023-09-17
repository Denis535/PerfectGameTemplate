#nullable enable
namespace UnityEngine.Framework.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UnityEngine;

    public abstract class UIWidgetBase : IUILogicalElement, IUIObservable, IDisposable {

        // System
        private Lock Lock { get; } = new Lock();
        public bool IsDisposed { get; private protected set; }
        public virtual bool DisposeAutomatically => true;
        // Observable
        public Action<UIMessage>? OnMessageEvent { get; set; }
        public UIMessageDispatcher Dispatcher => new UIMessageDispatcher( this );
        // View
        [MemberNotNullWhen( true, "View" )] public bool IsViewable => IsViewableInternal;
        public UIWidgetViewBase? View => ViewInternal;
        // View/Internal
        [MemberNotNullWhen( true, "ViewInternal" )] protected private virtual bool IsViewableInternal => false;
        protected private virtual UIWidgetViewBase? ViewInternal => null;
        // State
        public UIWidgetState State { get; private set; } = UIWidgetState.Unattached;
        [MemberNotNullWhen( true, "Screen" )] public bool IsAttached => State is UIWidgetState.Attaching or UIWidgetState.Attached or UIWidgetState.Detaching;
        [MemberNotNullWhen( true, "Screen" )] public bool IsAttachedStrict => State is UIWidgetState.Attached;
        [MemberNotNullWhen( false, "Screen" )] public bool IsNonAttached => State is UIWidgetState.Unattached or UIWidgetState.Detached;
        // Owner
        internal IUILogicalElement? Owner => (IUILogicalElement?) Parent ?? Screen;
        // Screen
        public UIScreenBase? Screen { get; private set; }
        // Parent
        [MemberNotNullWhen( false, "Parent" )] public bool IsRoot => Parent == null;
        public UIWidgetBase? Parent { get; internal set; }
        public IReadOnlyList<UIWidgetBase> Ancestors => this.GetAncestors();
        public IReadOnlyList<UIWidgetBase> AncestorsAndSelf => this.GetAncestorsAndSelf();
        // Children
        public bool HasChildren => Children_.Any();
        private List<UIWidgetBase> Children_ { get; } = new List<UIWidgetBase>();
        public IReadOnlyList<UIWidgetBase> Children => Children_;
        public IReadOnlyList<UIWidgetBase> Descendants => this.GetDescendants();
        public IReadOnlyList<UIWidgetBase> DescendantsAndSelf => this.GetDescendantsAndSelf();
        // OnAttach
        public Action? OnAttachEvent { get; set; }
        public Action? OnDetachEvent { get; set; }
        // OnDescendantAttach
        public Action<UIWidgetBase>? OnBeforeDescendantAttachEvent { get; set; }
        public Action<UIWidgetBase>? OnAfterDescendantAttachEvent { get; set; }
        public Action<UIWidgetBase>? OnBeforeDescendantDetachEvent { get; set; }
        public Action<UIWidgetBase>? OnAfterDescendantDetachEvent { get; set; }

        // Constructor
        public UIWidgetBase() {
        }
        public virtual void Dispose() {
            Assert.Object.Message( $"Widget {this} must be alive" ).Alive( !IsDisposed );
            Assert.Object.Message( $"Widget {this} must be non-attached" ).Valid( IsNonAttached );
            IsDisposed = true;
        }

        // Attach
        internal void Attach(UIScreenBase screen) {
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen );
            Assert.Object.Message( $"Widget {this} must be non-attached" ).Valid( IsNonAttached );
            Assert.Object.Message( $"Widget {this} must be valid" ).Valid( Screen == null );
            State = UIWidgetState.Attaching;
            Screen = screen;
            OnBeforeDescendantAttach( Owner!, this );
            {
                OnAttachEvent?.Invoke();
                OnAttach();
                Screen!.ShowWidget( this );
                OnShow();
                foreach (var child in Children) {
                    child.Attach( Screen );
                }
            }
            OnAfterDescendantAttach( Owner!, this );
            State = UIWidgetState.Attached;
        }
        internal void Detach(UIScreenBase screen) {
            Assert.Argument.Message( $"Argument 'screen' must be non-null" ).NotNull( screen );
            Assert.Object.Message( $"Widget {this} must be attached" ).Valid( IsAttached );
            Assert.Object.Message( $"Widget {this} must be valid" ).Valid( Screen == screen );
            State = UIWidgetState.Detaching;
            OnBeforeDescendantDetach( Owner!, this );
            {
                foreach (var child in Children.Reverse()) {
                    child.Detach( Screen );
                }
                OnHide();
                Screen!.HideWidget( this );
                OnDetach();
                OnDetachEvent?.Invoke();
            }
            OnAfterDescendantDetach( Owner!, this );
            Screen = null;
            State = UIWidgetState.Detached;
            if (DisposeAutomatically) {
                Dispose();
            }
        }

        // OnAttach
        public abstract void OnAttach();
        public abstract void OnDetach();

        // OnShow
        public abstract void OnShow();
        public abstract void OnHide();

        // AttachChild
        protected internal virtual void __AttachChild__(UIWidgetBase child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Argument.Message( $"Argument 'child' ({child}) must be non-attached" ).Valid( child.IsNonAttached );
            Assert.Argument.Message( $"Argument 'child' ({child}) must be valid" ).Valid( child.Screen == null );
            Assert.Argument.Message( $"Argument 'child' ({child}) must be valid" ).Valid( child.Parent == null );
            Assert.Object.Message( $"Widget {this} must have no child {child} widget" ).Valid( !Children.Contains( child ) );
            using (Lock.Enter()) {
                Children_.Add( child );
                child.Parent = this;
                if (IsAttached) {
                    child.Attach( Screen );
                }
            }
        }
        protected internal virtual void __DetachChild__(UIWidgetBase child) {
            Assert.Argument.Message( $"Argument 'child' must be non-null" ).NotNull( child != null );
            Assert.Argument.Message( $"Argument 'child' ({child}) must be attached or non-attached" ).Valid( child.IsAttached || child.IsNonAttached );
            Assert.Argument.Message( $"Argument 'child' ({child}) must be valid" ).Valid( child.Screen == Screen || child.Screen == null );
            Assert.Argument.Message( $"Argument 'child' ({child}) must be valid" ).Valid( child.Parent == this );
            Assert.Object.Message( $"Widget {this} must have child {child} widget" ).Valid( Children.Contains( child ) );
            using (Lock.Enter()) {
                if (IsAttached) {
                    child.Detach( Screen );
                }
                child.Parent = null;
                Children_.Remove( child );
            }
        }

        // OnDescendantAttach
        public virtual void OnBeforeDescendantAttach(UIWidgetBase descendant) {
            OnBeforeDescendantAttach( Owner!, descendant );
        }
        public virtual void OnAfterDescendantAttach(UIWidgetBase descendant) {
            OnAfterDescendantAttach( Owner!, descendant );
        }
        public virtual void OnBeforeDescendantDetach(UIWidgetBase descendant) {
            OnBeforeDescendantDetach( Owner!, descendant );
        }
        public virtual void OnAfterDescendantDetach(UIWidgetBase descendant) {
            OnAfterDescendantDetach( Owner!, descendant );
        }

        // Helpers/OnDescendantAttach
        private static void OnBeforeDescendantAttach(IUILogicalElement element, UIWidgetBase descendant) {
            if (element is UIWidgetBase parent) {
                parent.OnBeforeDescendantAttachEvent?.Invoke( descendant );
                parent.OnBeforeDescendantAttach( descendant );
            } else
            if (element is UIScreenBase screen) {
                screen.OnBeforeDescendantWidgetAttachEvent?.Invoke( descendant );
                screen.OnBeforeDescendantWidgetAttach( descendant );
            }
        }
        private static void OnAfterDescendantAttach(IUILogicalElement element, UIWidgetBase descendant) {
            if (element is UIWidgetBase parent) {
                parent.OnAfterDescendantAttach( descendant );
                parent.OnAfterDescendantAttachEvent?.Invoke( descendant );
            } else
            if (element is UIScreenBase screen) {
                screen.OnAfterDescendantWidgetAttach( descendant );
                screen.OnAfterDescendantWidgetAttachEvent?.Invoke( descendant );
            }
        }
        private static void OnBeforeDescendantDetach(IUILogicalElement element, UIWidgetBase descendant) {
            if (element is UIWidgetBase parent) {
                parent.OnBeforeDescendantDetachEvent?.Invoke( descendant );
                parent.OnBeforeDescendantDetach( descendant );
            } else
            if (element is UIScreenBase screen) {
                screen.OnBeforeDescendantWidgetDetachEvent?.Invoke( descendant );
                screen.OnBeforeDescendantWidgetDetach( descendant );
            }
        }
        private static void OnAfterDescendantDetach(IUILogicalElement element, UIWidgetBase descendant) {
            if (element is UIWidgetBase parent) {
                parent.OnAfterDescendantDetach( descendant );
                parent.OnAfterDescendantDetachEvent?.Invoke( descendant );
            } else
            if (element is UIScreenBase screen) {
                screen.OnAfterDescendantWidgetDetach( descendant );
                screen.OnAfterDescendantWidgetDetachEvent?.Invoke( descendant );
            }
        }

    }
    public abstract class UIWidgetBase<TView> : UIWidgetBase where TView : notnull, UIWidgetViewBase {

        private TView view = default!;

        // View
        public new bool IsViewable => base.IsViewable;
        public new TView View {
            get => (TView) base.View!;
            protected init {
                view = value;
                view.Widget = this;
            }
        }
        // View/Internal
        protected private override bool IsViewableInternal => true;
        protected private override UIWidgetViewBase? ViewInternal => view;

        // Constructor
        public UIWidgetBase() {
        }
        public override void Dispose() {
            Assert.Object.Message( $"Widget {this} must be alive" ).Alive( !IsDisposed );
            Assert.Object.Message( $"Widget {this} must be non-attached" ).Valid( IsNonAttached );
            IsDisposed = true;
            View.Dispose();
        }

        // AttachChild
        protected internal override void __AttachChild__(UIWidgetBase child) {
            base.__AttachChild__( child );
        }
        protected internal override void __DetachChild__(UIWidgetBase child) {
            base.__DetachChild__( child );
        }

        // OnDescendantAttach
        public override void OnBeforeDescendantAttach(UIWidgetBase descendant) {
            base.OnBeforeDescendantAttach( descendant );
        }
        public override void OnAfterDescendantAttach(UIWidgetBase descendant) {
            base.OnAfterDescendantAttach( descendant );
        }
        public override void OnBeforeDescendantDetach(UIWidgetBase descendant) {
            base.OnBeforeDescendantDetach( descendant );
        }
        public override void OnAfterDescendantDetach(UIWidgetBase descendant) {
            base.OnAfterDescendantDetach( descendant );
        }

    }
}
