﻿//
//    By Keith from http://stackoverflow.com/questions/2416748/how-to-simulate-mouse-click-in-c
//    With modifications.

using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
namespace MagicXboxOneController
{
    public class MouseOperations
    {
        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Wheel = 0x00000800,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
            
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public static void SetCursorPosition(int X, int Y)
        {
            SetCursorPos(X, Y);
        }

        public static void SetCursorPosition(MousePoint point)
        {
            SetCursorPos(point.X, point.Y);
        }

        public static void SetCursorPosition(Vector2 pos)
        {
            SetCursorPos((int)pos.X, (int)pos.Y);
        }

        public static void MouseMove(Vector2 delta)
        {
            mouse_event((int)MouseEventFlags.Move, (int)delta.X, (int)delta.Y, 0,0);
        }

        public static MousePoint GetCursorPosition()
        {
            MousePoint currentMousePoint;
            var gotPoint = GetCursorPos(out currentMousePoint);
            if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
            return currentMousePoint;
        }

        public static void MouseEvent(MouseEventFlags value)
        {
            MouseEvent(value, 0, 0);
        }

        public static void MouseEvent(MouseEventFlags value, int dwData, int dwExtraInfo)
        {
            MousePoint position = GetCursorPosition();

            mouse_event
                ((int)value,
                 position.X,
                 position.Y,
                 dwData,
                 dwExtraInfo)
                ;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint
        {
            public int X;
            public int Y;

            public MousePoint(int x, int y)
            {
                X = x;
                Y = y;
            }

            public Vector2 ToVector2()
            {
                return new Vector2(X, Y);
            }
        }


    }
}