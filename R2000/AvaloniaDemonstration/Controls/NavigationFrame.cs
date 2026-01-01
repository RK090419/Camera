using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;

namespace AvaloniaDemonstration.Controls;

[TemplatePart("PART_ContentPresenter2", typeof(ContentPresenter))]
public sealed class NavigationFrame : TransitioningContentControl
{
    readonly LinkedList<object> _history = new();
    LinkedListNode<object>? _currentNode;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var presenter = e.NameScope.Find<ContentPresenter>("PART_ContentPresenter2") ??
            throw new InvalidOperationException
            ($"{nameof(NavigationFrame)} requires a ContentPresenter named 'PART_ContentPresenter2' in its ControlTemplate.");

        RegisterContentPresenter(presenter);
    }

    public void Navigate(object? newContent)
    {
        Dispatcher.UIThread.VerifyAccess();

        if (newContent is null)
        {
            ClearHistory();
            Content = null;
            return;
        }

        LinkedListNode<object>? existingNode = null;

        if (_currentNode is null)
        {
            _currentNode = _history.AddFirst(newContent);
        }
        else
        {
            existingNode = _history.Find(newContent);

            if (existingNode is not null)
            {
                _currentNode = existingNode;
            }
            else
            {
                if (_currentNode.Next is not null)
                {
                    var node = _currentNode.Next;
                    while (node is not null)
                    {
                        var nextNode = node.Next;
                        _history.Remove(node);
                        node = nextNode;
                    }
                }
                _currentNode = _history.AddAfter(_currentNode, newContent);
            }
        }

        Content = newContent;
    }

    public void ClearHistory()
    {
        Dispatcher.UIThread.VerifyAccess();

        _history.Clear();
        _currentNode = null;
    }
}