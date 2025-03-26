﻿namespace AutoSystem.Models
{
    public class StepDataModel
    {
        public int Id { get; set; }
        public int ModeId { get; set; }
        public int Timer { get; set; }
        public string Destination { get; set; }
        public int Speed { get; set; }
        public string Type { get; set; }
        public int Volume { get; set; }

        public ModeDataModel Mode { get; set; }
    }
}
