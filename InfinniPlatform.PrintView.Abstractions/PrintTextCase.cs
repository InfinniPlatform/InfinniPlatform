﻿using System;

namespace InfinniPlatform.PrintView
{
    /// <summary>
    /// Регистр символов текста элемента.
    /// </summary>
    [Serializable]
    public enum PrintTextCase
    {
        /// <summary>
        /// Текст элемента печатного представления остается без изменений.
        /// </summary>
        /// <example>
        /// "Какой-то текст" → "Какой-то текст".
        /// </example>
        Normal,

        /// <summary>
        /// Текст интерпретируется, как предложение, которое должно начинаться с заглавной буквы.
        /// </summary>
        /// <example>
        /// "текст некоторого предложения" → "Текст некоторого предложения".
        /// </example>
        SentenceCase,

        /// <summary>
        /// Текст приводится к нижнему регистру.
        /// </summary>
        /// <example>
        /// "КАКОЙ-ТО ТЕКСТ" → "какой-то текст".
        /// </example>
        Lowercase,

        /// <summary>
        /// Текст приводится к верхнему регистру.
        /// </summary>
        /// <example>
        /// "какой-то текст" → "КАКОЙ-ТО ТЕКСТ".
        /// </example>
        Uppercase,

        /// <summary>
        /// Текст меняет регистр символов.
        /// </summary>
        /// <example>
        /// "какой-то ТЕКСТ" → "КАКОЙ-ТО текст", "КАКОЙ-ТО текст" → "какой-то ТЕКСТ".
        /// </example>
        ToggleCase
    }
}