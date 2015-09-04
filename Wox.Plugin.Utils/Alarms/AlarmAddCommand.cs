﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wox.Plugin.Utils.Alarms
{
    public class AlarmAddCommand : CommandHandlerBase
    {
        public AlarmAddCommand(PluginInitContext context, CommandHandlerBase parent) : base(context, parent) { }

        public override string CommandAlias
        {
            get
            {
                return "add";
            }
        }

        public override string CommandDescription
        {
            get
            {
                return "Adds a new alarm";
            }
        }

        public override string CommandTitle
        {
            get
            {
                return "Add new alarm";
            }
        }

        public override string GetIconPath()
        {
            return "Images\\alarm-green.png";
        }

        public override bool ExecuteCommand(List<string> args)
        {
            if (args.Count > _commandDepth)
            {
                try
                {
                    var time = DateTime.ParseExact(args[_commandDepth], "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    if (time < DateTime.Now) time = time.AddDays(1);

                    var name = "Alarm";
                    if (args.Count > _commandDepth + 1)
                    {
                        name = String.Join(" ", args.Skip(_commandDepth + 1).ToArray());
                    }

                    AlarmStorage.Instance.Alarms.Add(new AlarmStorage.StoredAlarm(true)
                    {
                        AlarmTime = time,
                        Name = name
                    });
                    AlarmStorage.Instance.SaveAlarms();
                    RequeryWithArguments(args);

                    _forcedSubtitle = String.Format("\"{0}\" will fire at {1}", name, time.ToString());
                    _forcedTitle = "Alarm set!";
                    RequeryCurrentCommand();
                    
                    return false;
                }
                catch (FormatException e)
                {
                    _forcedSubtitle = "Time format invalid.";
                }
            }
            RequeryWithArguments(args);
            return false;
        }

        public override List<Result> Query(Query query)
        {
            var res = new List<Result>();
            var args = query.ActionParameters;
            res.Add(new Result()
            {
                Title = String.IsNullOrEmpty(_forcedTitle) ? "You are adding a new alarm" : _forcedTitle,
                SubTitle = String.IsNullOrEmpty(_forcedSubtitle) ? "Accepts: time as HH:MM, name as any string" : _forcedSubtitle,
                IcoPath = GetIconPath(),
                Action = e =>
                {
                    return ExecuteCommand(args);
                }
            });

            return res;
        }
    }
}