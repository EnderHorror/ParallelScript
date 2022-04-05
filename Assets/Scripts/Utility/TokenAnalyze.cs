using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;

/// <summary>
/// 词法分析
/// </summary>
public class TokenAnalyze
{
    private const string REMAINDER_REGEX = @"(.*?((?=>)|(/|$)))";
    private const string PAUSE_REGEX_STRING = "<p:(?<pause>" + REMAINDER_REGEX + ")>";
    private static readonly Regex pauseRegex = new Regex(PAUSE_REGEX_STRING);
    private const string SPEED_REGEX_STRING = "<sp:(?<speed>" + REMAINDER_REGEX + ")>";
    private static readonly Regex speedRegex = new Regex(SPEED_REGEX_STRING);
    private const string ANIM_START_REGEX_STRING = "<anim:(?<anim>" + REMAINDER_REGEX + ")>";
    private static readonly Regex animStartRegex = new Regex(ANIM_START_REGEX_STRING);
    private const string ANIM_END_REGEX_STRING = "</anim>";
    private static readonly Regex animEndRegex = new Regex(ANIM_END_REGEX_STRING);

    private const string LUA_REGEX_STRING = "<lua:(?<lua>" + REMAINDER_REGEX + ")>";
    private static readonly Regex luaRegex = new Regex(LUA_REGEX_STRING);


    public static List<DialogueCommand> ProcessInputString(string message, out string processedMessage)
    {
        List<DialogueCommand> result = new List<DialogueCommand>();
        processedMessage = message;
        processedMessage = HandleLuaTags(processedMessage,result);
        processedMessage = HandlePauseTags(processedMessage, result);
        processedMessage = HandleAnimStartTags(processedMessage, result);
        processedMessage = HandleAnimEndTags(processedMessage, result);
        foreach (DialogueCommand command in result)
        {
            Debug.Log(command.ToString());
        }
        return result;
    }
    private static string HandleLuaTags(string processedMessage, List<DialogueCommand> result)
    {
        MatchCollection pauseMatches = luaRegex.Matches(processedMessage);
        foreach (Match match in pauseMatches)
        {
            string val = match.Groups["lua"].Value;
            string dostring = val;
            result.Add(new DialogueCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = DialogueCommandType.Lua,
                stringValue = dostring,
            });
        }
        processedMessage = Regex.Replace(processedMessage, LUA_REGEX_STRING, "");
        return processedMessage;
    }

    private static string HandlePauseTags(string processedMessage, List<DialogueCommand> result)
    {
        MatchCollection pauseMatches = pauseRegex.Matches(processedMessage);
        foreach (Match match in pauseMatches)
        {
            string val = match.Groups["pause"].Value;
            string pauseName = val;
            result.Add(new DialogueCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = DialogueCommandType.Pause,
                floatValue = float.Parse(val),
            });
        }
        processedMessage = Regex.Replace(processedMessage, PAUSE_REGEX_STRING, "");
        return processedMessage;
    }

    private static string HandleAnimStartTags(string processedMessage, List<DialogueCommand> result)
    {
        MatchCollection animStartMatches = animStartRegex.Matches(processedMessage);
        foreach (Match match in animStartMatches)
        {
            string stringVal = match.Groups["anim"].Value;
            result.Add(new DialogueCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = DialogueCommandType.AnimStart,
                textAnimValue = ParallelScriptFormater.GetTextAnimationType(stringVal)
            });
        }
        processedMessage = Regex.Replace(processedMessage, ANIM_START_REGEX_STRING, "");
        return processedMessage;
    }

    private static string HandleAnimEndTags(string processedMessage, List<DialogueCommand> result)
    {
        MatchCollection animEndMatches = animEndRegex.Matches(processedMessage);
        foreach (Match match in animEndMatches)
        {
            result.Add(new DialogueCommand
            {
                position = VisibleCharactersUpToIndex(processedMessage, match.Index),
                type = DialogueCommandType.AnimEnd,
            });
        }
        processedMessage = Regex.Replace(processedMessage, ANIM_END_REGEX_STRING, "");
        return processedMessage;
    }


    /// <summary>
    /// 把文本文件的字符切分成一行一行的
    /// </summary>
    public static Queue<string> ClipLine(string text)
    {
        var buffers = Regex.Split(text, @"\n");
        return new Queue<string>(buffers);
    }
    private static int VisibleCharactersUpToIndex(string message, int index)
    {
        int result = 0;
        bool insideBrackets = false;
        for (int i = 0; i < index; i++)
        {
            if (message[i] == '<')
            {
                insideBrackets = true;
            }
            else if (message[i] == '>')
            {
                insideBrackets = false;
                result--;
            }
            if (!insideBrackets)
            {
                result++;
            }

        }
        return result;
    }
}
public struct DialogueCommand
{
    public int position;
    public DialogueCommandType type;
    public float floatValue;
    public string stringValue;
    public TextAnimationType textAnimValue;

    public override string ToString()
    {
        return $"Position:{position}\n" +
            $"Type:{type}\n" +
            $"float:{floatValue}\n" +
            $"string:{stringValue}\n" +
            $"Anim:{textAnimValue}";
    }
}

public enum DialogueCommandType
{
    Pause,
    Lua,
    TextSpeedChange,
    AnimStart,
    AnimEnd
}

