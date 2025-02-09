using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SirenDisplay.Assets.Polygons.Buttons;
using SirenDisplay.ViewModels;
using SirenDisplay.Views;

namespace SirenDisplay;

public sealed class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;


        var viewName = data.GetType().FullName!.Replace("ViewModel", "View", StringComparison.InvariantCulture);
        var type = Type.GetType(viewName);

        if (type is null)
            return null;

        var control = (Control)Activator.CreateInstance(type)!;
        control.DataContext = data;
        return control;
    }

    public bool Match(object? data) => data is ViewModelBase;
}