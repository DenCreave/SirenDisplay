using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SirenDisplay.Model;

public sealed class DirectoryItem : ObservableObject
{
    public bool IsMusic { get; set; } // this is the phospor icon: directory, music, sth else; 
    public string Label { get; set; }
    public string Name { get; set; } //of song/dir
    public string FullPath { get; set; }

    public DirectoryItem(){ }

    public DirectoryItem(DirectoryItem directoryItem)
    {
        IsMusic = directoryItem.IsMusic;
        Label = directoryItem.Label;
        Name = directoryItem.Name;
        FullPath = directoryItem.FullPath;
    }
}