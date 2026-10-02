using System;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }

    class Game
    {
        private char[] _board;
        private char _currentPlayer;
        private int _movesCount;

        public Game()
        {
            _board = new char[9];
            Reset();
        }

        /// <summary>
        /// Запуск основного цикла игры с возможностью рестарта.
        /// </summary>
        public void Run()
        {
            bool playAgain = true;

            while (playAgain)
            {
                Reset();
                PlayRound();

                Console.Write("\nХотите сыграть ещё раз? (y/n): ");
                string answer = Console.ReadLine()?.Trim().ToLower();
                playAgain = answer == "y" || answer == "д";
            }

            Console.WriteLine("Спасибо за игру!");
        }

        /// <summary>
        /// Сброс состояния игры.
        /// </summary>
        private void Reset()
        {
            for (int i = 0; i < _board.Length; i++)
            {
                _board[i] = (char)('1' + i); // поля 1..9
            }
            _currentPlayer = 'X';
            _movesCount = 0;
        }

        /// <summary>
        /// Один раунд игры до победы, ничьей или выхода.
        /// </summary>
        private void PlayRound()
        {
            Console.WriteLine("=== Крестики-нолики ===");
            Console.WriteLine("Игрок X ходит первым.");
            Console.WriteLine("Введите номер клетки (1-9), чтобы сделать ход.");

            while (true)
            {
                DrawBoard();
                Console.WriteLine($"\nХод игрока '{_currentPlayer}'.");

                if (!TryGetMove(out int cell))
                {
                    Console.WriteLine("Выход из игры...");
                    return;
                }

                MakeMove(cell);

                if (CheckWin(_currentPlayer))
                {
                    DrawBoard();
                    Console.WriteLine($"\nПобедил игрок '{_currentPlayer}'! Поздравляем!");
                    return;
                }

                if (_movesCount == 9)
                {
                    DrawBoard();
                    Console.WriteLine("\nНичья! Все клетки заполнены.");
                    return;
                }

                SwitchPlayer();
            }
        }

        /// <summary>
        /// Отрисовка игрового поля.
        /// </summary>
        private void DrawBoard()
        {
            Console.WriteLine();
            Console.WriteLine($" {_board[0]} | {_board[1]} | {_board[2]} ");
            Console.WriteLine("---+---+---");
            Console.WriteLine($" {_board[3]} | {_board[4]} | {_board[5]} ");
            Console.WriteLine("---+---+---");
            Console.WriteLine($" {_board[6]} | {_board[7]} | {_board[8]} ");
        }

        /// <summary>
        /// Проверка корректности ввода и занятости клетки.
        /// </summary>
        private bool TryGetMove(out int cellIndex)
        {
            cellIndex = -1;
            Console.Write("Ваш ход (1-9, 0 - выход): ");
            string input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Ошибка: пустой ввод. Попробуйте ещё раз.");
                return false;
            }

            if (!int.TryParse(input, out int number))
            {
                Console.WriteLine("Ошибка: необходимо ввести число. Попробуйте ещё раз.");
                return false;
            }

            if (number == 0)
            {
                return false;
            }

            if (number < 1 || number > 9)
            {
                Console.WriteLine("Ошибка: число должно быть в диапазоне 1-9.");
                return false;
            }

            int index = number - 1;
            if (_board[index] == 'X' || _board[index] == 'O')
            {
                Console.WriteLine("Ошибка:эта клетка уже занята.Выберите другую.");
                return false;
            }

            cellIndex = index;
            return true;
        }

        /// <summary>
        /// Установка знака текущего игрока в выбранную клетку.
        /// </summary>
        private void MakeMove(int cellIndex)
        {
            _board[cellIndex] = _currentPlayer;
            _movesCount++;
        }

        /// <summary>
        /// Смена активного игрока.
        /// </summary>
        private void SwitchPlayer()
        {
            _currentPlayer = _currentPlayer == 'X' ? 'O' : 'X';
        }

        /// <summary>
        /// Проверка победы: горизонтали, вертикали, диагонали.
        /// </summary>
        private bool CheckWin(char player)
        {
            // Выигрышные комбинации индексов
            int[][] winningCombinations =
            {
                new[] { 0, 1, 2 }, // верхняя горизонталь
                new[] { 3, 4, 5 }, // средняя горизонталь
                new[] { 6, 7, 8 }, // нижняя горизонталь
                new[] { 0, 3, 6 }, // левая вертикаль
                new[] { 1, 4, 7 }, // центральная вертикаль
                new[] { 2, 5, 8 }, // правая вертикаль
                new[] { 0, 4, 8 }, // диагональ сверху-слева вниз-справа
                new[] { 2, 4, 6 }  // диагональ сверху-справа вниз-слева
            };

            foreach (var combo in winningCombinations)
            {
                if (_board[combo[0]] == player &&
                    _board[combo[1]] == player &&
                    _board[combo[2]] == player)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

