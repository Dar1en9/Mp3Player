﻿using Mp3Player.TrackHandler;

namespace Mp3Player.DataBase;

public interface IDataBaseWriter
{
    Task WriteTrack(Track track);
}