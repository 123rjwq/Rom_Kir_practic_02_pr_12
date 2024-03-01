using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace pr_11
{
    public partial class SudokuForm : Form
    {
        private int[,] backupSudoku; // Свойство для хранения резервного копирования судоку

        // Массив цветов для блоков 3x3 в судоку
        private Color[,] blockColors = {
            { Color.Red, Color.Orange, Color.Yellow },
            { Color.Green, Color.Blue, Color.Indigo },
            { Color.Violet, Color.Gray, Color.White }
        };

        // Двумерный массив текстовых полей для судоку
        private TextBox[,] sudokuTextBoxes;

        public SudokuForm()
        {
            // Инициализация текстовых полей для судоку
            InitializeSudokuTextBoxes();
            // Установка цвета для всех блоков 3x3
            ColorAllSudokuBlocks();

            // Создание кнопки для генерации судоку
            CreateGenerateButton();
            // Создание кнопки для очистки полей судоку
            CreateClearButton();
            // Создание кнопки для сохранения судоку в файл
            SaveButton();
            // Создание кнопки для загрузки судоку из файла
            LoadSudokuButton();
            // Создание кнопки для разрешения ввода для пользователя
            EnableInputButton();
            // Создание кнопки для запрета ввода для пользователя
            DisableInputButton();
            // Создание кнопки для проверки введенных значений
            CheckButton();
            // Создание кнопки для отмены проверки
            UndoButton();
            // Создание кнопки для возвращения исходных цветов судоку
            ResetColorsButton();

            // Установка размеров формы
            this.Width = 500;
            this.Height = 500;
        }

        // Инициализация текстовых полей для судоку
        private void InitializeSudokuTextBoxes()
        {
            sudokuTextBoxes = new TextBox[9, 9];
            int textBoxSize = 30;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Size = new System.Drawing.Size(textBoxSize, textBoxSize);
                    textBox.Location = new System.Drawing.Point(col * (textBoxSize + 3), row * (textBoxSize + 3));
                    textBox.TextAlign = HorizontalAlignment.Center;

                    textBox.ReadOnly = true;
                    textBox.Multiline = true;
                    textBox.Font = new Font(textBox.Font.FontFamily, 16);

                    // Установка текста ячейки в "0"
                    textBox.Text = "0";

                    this.Controls.Add(textBox);
                    sudokuTextBoxes[row, col] = textBox;
                }
            }
        }



        // Установка цвета для блока 3x3 в судоку
        private void ColorSudokuBlock(int blockRow, int blockCol)
        {
            for (int row = blockRow * 3; row < (blockRow + 1) * 3; row++)
            {
                for (int col = blockCol * 3; col < (blockCol + 1) * 3; col++)
                {
                    sudokuTextBoxes[row, col].BackColor = blockColors[blockRow, blockCol];
                    sudokuTextBoxes[row, col].BorderStyle = BorderStyle.FixedSingle;
                }
            }
        }

        // Установка цвета для всех блоков 3x3 в судоку
        private void ColorAllSudokuBlocks()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    ColorSudokuBlock(row, col);
                }
            }
        }

        // Создание кнопки для сбоса цветов
        private void ResetColorsButton()
        {
            Button btnResetColors = new Button();
            btnResetColors.Text = "Сбросить цвета";
            btnResetColors.Size = new Size(150, 30);
            btnResetColors.Location = new Point(300, 330); // Расположение в зависимости от других элементов
            btnResetColors.Click += new EventHandler(btnResetColors_Click);
            this.Controls.Add(btnResetColors);

        }

        // Создание кнопки для генерации судоку
        private void CheckButton()
        {
            Button btnCheck = new Button();
            btnCheck.Text = "Проверить судоку";
            btnCheck.Size = new Size(150, 30);
            btnCheck.Location = new Point(300, 250);
            btnCheck.Click += new EventHandler(btnCheckSudoku_Click);
            this.Controls.Add(btnCheck);
        }

        // Создание кнопки для генерации судоку
        private void UndoButton()
        {
            Button btnUndo = new Button();
            btnUndo.Text = "Отменить изменения";
            btnUndo.Size = new Size(150, 30);
            btnUndo.Location = new Point(300, 290);
            btnUndo.Click += new EventHandler(btnUndoChanges_Click);
            this.Controls.Add(btnUndo);
        }

        // Создание кнопки для генерации судоку
        private void EnableInputButton()
        {
            Button btnEnableInput = new Button();
            btnEnableInput.Text = "Разрешить ввод";
            btnEnableInput.Size = new Size(150, 30);
            btnEnableInput.Location = new Point(300, 170);
            btnEnableInput.Click += new EventHandler(btnEnableInput_Click);
            this.Controls.Add(btnEnableInput);
        }

        // Создание кнопки для генерации судоку
        private void DisableInputButton()
        {
            Button btnDisableInput = new Button();
            btnDisableInput.Text = "Запретить ввод";
            btnDisableInput.Size = new Size(150, 30);
            btnDisableInput.Location = new Point(300, 210);
            btnDisableInput.Click += new EventHandler(btnDisableInput_Click);
            this.Controls.Add(btnDisableInput);
        }

        // Создание кнопки для генерации судоку
        private void CreateGenerateButton()
        {
            Button generateButton = new Button();
            generateButton.Text = "Сгенерировать судоку";
            generateButton.Size = new Size(150, 30);
            generateButton.Location = new Point(300, 9);
            generateButton.Click += new EventHandler(GenerateSudokuButton_Click);

            this.Controls.Add(generateButton);
        }

        // Создание кнопки для очистки полей судоку
        private void CreateClearButton()
        {
            Button clearButton = new Button();
            clearButton.Text = "Очистить поля";
            clearButton.Size = new Size(150, 30);
            clearButton.Location = new Point(300, 50);
            clearButton.Click += new EventHandler(ClearSudokuFieldsButton_Click);

            this.Controls.Add(clearButton);
        }

        // Создание кнопки для сохранения судоку в файл
        private void SaveButton()
        {
            Button btnSave = new Button();
            btnSave.Text = "Сохранить судоку";
            btnSave.Size = new Size(150, 30);
            btnSave.Location = new Point(300, 90);
            btnSave.Click += new EventHandler(btnSaveSudoku_Click);

            this.Controls.Add(btnSave);
        }

        // Создание кнопки для загрузки судоку из файла
        private void LoadSudokuButton()
        {
            Button btnLoad = new Button();
            btnLoad.Text = "Загрузить судоку";
            btnLoad.Size = new Size(150, 30);
            btnLoad.Location = new Point(300, 130);
            btnLoad.Click += new EventHandler(btnLoadSudoku_Click);

            this.Controls.Add(btnLoad);
        }

        // Генерирование судоку
        private void GenerateSudoku()
        {
            int[,] sudoku = new int[9, 9];
            GenerateSudokuRecursively(sudoku);
            DisplaySudoku(sudoku);
        }

        // Рекурсивный метод для генерации судоку
        private bool GenerateSudokuRecursively(int[,] sudoku)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (sudoku[row, col] == 0)
                    {
                        for (int num = 1; num <= 9; num++)
                        {
                            if (IsSafe(sudoku, row, col, num))
                            {
                                sudoku[row, col] = num;
                                if (GenerateSudokuRecursively(sudoku))
                                    return true;
                                sudoku[row, col] = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        // Проверка, безопасно ли установить число в данную ячейку
        private bool IsSafe(int[,] sudoku, int row, int col, int num)
        {
            return !UsedInRow(sudoku, row, num) && !UsedInCol(sudoku, col, num) && !UsedInBox(sudoku, row - row % 3, col - col % 3, num);
        }

        // Проверка, использовалось ли число в данной строке
        private bool UsedInRow(int[,] sudoku, int row, int num)
        {
            for (int col = 0; col < 9; col++)
            {
                if (sudoku[row, col] == num)
                    return true;
            }
            return false;
        }

        // Проверка, использовалось ли число в данном столбце
        private bool UsedInCol(int[,] sudoku, int col, int num)
        {
            for (int row = 0; row < 9; row++)
            {
                if (sudoku[row, col] == num)
                    return true;
            }
            return false;
        }

        // Проверка, использовалось ли число в данном блоке 3x3
        private bool UsedInBox(int[,] sudoku, int boxStartRow, int boxStartCol, int num)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (sudoku[row + boxStartRow, col + boxStartCol] == num)
                        return true;
                }
            }
            return false;
        }

        // Отображение сгенерированного судоку
        private void DisplaySudoku(int[,] sudoku)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    sudokuTextBoxes[row, col].Text = sudoku[row, col].ToString();
                }
            }
        }

        // Обработчик события кнопки "Сгенерировать судоку"
        private void GenerateSudokuButton_Click(object sender, EventArgs e)
        {
            GenerateSudoku();
            MessageBox.Show("Судоку сгенерировано!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Обработчик события кнопки "Очистить поля"
        private void ClearSudokuFieldsButton_Click(object sender, EventArgs e)
        {
            ClearAllSudokuFields();
            MessageBox.Show("Поля очищены!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Очистка всех полей судоку
        private void ClearAllSudokuFields()
        {
            foreach (TextBox textBox in sudokuTextBoxes)
            {
                textBox.Text = "0";
            }
        }

        // Обработчик события для сохранения судоку в файл
        private void btnSaveSudoku_Click(object sender, EventArgs e)
        {
            // Создание диалогового окна для выбора файла для сохранения
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовый файл (*.txt)|*.txt";

            // Отображение диалогового окна и проверка, выбран ли файл
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Использование StreamWriter для записи данных в файл
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    // Запись каждой ячейки судоку в файл, разделяя значения пробелами
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            writer.Write(sudokuTextBoxes[i, j].Text + " ");
                        }
                        writer.WriteLine(); // Переход на новую строку после записи каждой строки судоку
                    }
                }
                // Отображение сообщения о том, что судоку сохранено
                MessageBox.Show("Судоку сохранено!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Обработчик события для загрузки судоку из файла
        private void btnLoadSudoku_Click(object sender, EventArgs e)
        {
            // Создание диалогового окна для выбора файла для загрузки
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовый файл (*.txt)|*.txt";

            // Отображение диалогового окна и проверка, выбран ли файл
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Использование StreamReader для чтения данных из файла
                using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                {
                    // Чтение каждой строки из файла и обновление соответствующих ячеек судоку
                    for (int i = 0; i < 9; i++)
                    {
                        string line = reader.ReadLine();
                        if (line != null)
                        {
                            string[] numbers = line.Split(' ');
                            for (int j = 0; j < 9; j++)
                            {
                                sudokuTextBoxes[i, j].Text = numbers[j];
                            }
                        }
                    }
                }
                // Отображение сообщения о том, что судоку загружено
                MessageBox.Show("Судоку загружено!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Обработчик события нажатия на кнопку "Разрешить ввод"
        private void btnEnableInput_Click(object sender, EventArgs e)
        {
            // Вызывает метод EnableInput, который разрешает пользователю вводить данные в текстовые поля судоку
            EnableInput();
        }

        // Обработчик события нажатия на кнопку "Запретить ввод"
        private void btnDisableInput_Click(object sender, EventArgs e)
        {
            // Вызывает метод DisableInput, который запрещает пользователю вводить данные в текстовые поля судоку
            DisableInput();
        }


        // Метод EnableInput активирует возможность ввода данных в текстовые поля судоку
        private void EnableInput()
        {
            // Проходим по всем текстовым полям судоку
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // Устанавливаем свойство ReadOnly в false, чтобы разрешить ввод данных
                    sudokuTextBoxes[i, j].ReadOnly = false;
                    // Подписываемся на событие KeyPress для каждого текстового поля, чтобы обрабатывать ввод пользователя
                    sudokuTextBoxes[i, j].KeyPress += new KeyPressEventHandler(sudokuTextBoxes_KeyPress);
                }
            }
        }


        // Метод DisableInput запрещает ввод данных в текстовые поля судоку
        private void DisableInput()
        {
            // Перебираем все текстовые поля судоку
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // Устанавливаем свойство ReadOnly в true, чтобы запретить ввод данных
                    sudokuTextBoxes[i, j].ReadOnly = true;
                }
            }
        }


        // Обработчик события KeyPress для текстовых полей судоку, позволяющий вводить только цифры от 1 до 9
        private void sudokuTextBoxes_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender; // Получаем текстовое поле, вызвавшее событие

            // Проверяем, что введенный символ является цифрой от 1 до 9 и что длина текста в поле не превышает 1 символ
            if (!char.IsDigit(e.KeyChar) || e.KeyChar < '1' || e.KeyChar > '9' || textBox.Text.Length >= 1)
            {
                e.Handled = true; // Если условие не выполняется, прерываем обработку события (ввод не допускается)
            }
        }


        // Метод CheckRowsAndColor проверяет наличие одинаковых чисел в каждой строке судоку и окрашивает строки в зависимости от результата проверки
        private void CheckRowsAndColor()
        {
            // Перебираем каждую строку судоку
            for (int row = 0; row < 9; row++)
            {
                // Создаем HashSet для хранения уникальных чисел в текущей строке
                var numbers = new HashSet<string>();

                // Проверяем каждую ячейку в текущей строке
                for (int col = 0; col < 9; col++)
                {
                    // Если ячейка не пустая
                    if (sudokuTextBoxes[row, col].Text.Length > 0)
                    {
                        // Попытка добавить число из ячейки в HashSet
                        // Если число уже присутствует в HashSet (то есть добавление не удалось),
                        // это означает, что в строке есть повторяющиеся числа
                        if (!numbers.Add(sudokuTextBoxes[row, col].Text))
                        {
                            // Окрашиваем строку в красный цвет, указывая на ошибку
                            ColorRow(row, Color.Red);
                            // Прерываем проверку текущей строки и переходим к следующей
                            return;
                        }
                    }
                }
                // Если строка проверена и в ней нет повторяющихся чисел, окрашиваем её в зелёный цвет
                ColorRow(row, Color.Green);
            }
        }


        // Метод CheckColumnsAndColor проверяет наличие одинаковых чисел в каждом столбце судоку и окрашивает столбцы в зависимости от результата проверки
        private void CheckColumnsAndColor()
        {
            // Перебираем каждый столбец судоку
            for (int col = 0; col < 9; col++)
            {
                // Создаем HashSet для хранения уникальных чисел в текущем столбце
                var numbers = new HashSet<string>();

                // Проверяем каждую ячейку в текущем столбце
                for (int row = 0; row < 9; row++)
                {
                    // Если ячейка не пустая
                    if (sudokuTextBoxes[row, col].Text.Length > 0)
                    {
                        // Попытка добавить число из ячейки в HashSet
                        // Если число уже присутствует в HashSet (то есть добавление не удалось),
                        // это означает, что в столбце есть повторяющиеся числа
                        if (!numbers.Add(sudokuTextBoxes[row, col].Text))
                        {
                            // Окрашиваем столбец в красный цвет, указывая на ошибку
                            ColorColumn(col, Color.Red);
                            // Прерываем проверку текущего столбца и переходим к следующему
                            return;
                        }
                    }
                }
                // Если столбец проверен и в нем нет повторяющихся чисел, окрашиваем его в зелёный цвет
                ColorColumn(col, Color.Green);
            }
        }


        // Метод CheckBoxesAndColor проверяет наличие одинаковых чисел в каждом квадрате 3x3 судоку и окрашивает квадраты в зависимости от результата проверки
        private void CheckBoxesAndColor()
        {
            // Перебираем каждый квадрат 3x3 в судоку
            for (int boxRow = 0; boxRow < 3; boxRow++)
            {
                for (int boxCol = 0; boxCol < 3; boxCol++)
                {
                    // Создаем HashSet для хранения уникальных чисел в текущем квадрате 3x3
                    var numbers = new HashSet<string>();

                    // Проверяем каждую ячейку в текущем квадрате 3x3
                    for (int row = boxRow * 3; row < boxRow * 3 + 3; row++)
                    {
                        for (int col = boxCol * 3; col < boxCol * 3 + 3; col++)
                        {
                            // Если ячейка не пустая
                            if (sudokuTextBoxes[row, col].Text.Length > 0)
                            {
                                // Попытка добавить число из ячейки в HashSet
                                // Если число уже присутствует в HashSet (то есть добавление не удалось),
                                // это означает, что в квадрате есть повторяющиеся числа
                                if (!numbers.Add(sudokuTextBoxes[row, col].Text))
                                {
                                    // Окрашиваем квадрат 3x3 в красный цвет, указывая на ошибку
                                    ColorBox(boxRow, boxCol, Color.Red);
                                    // Прерываем проверку текущего квадрата и переходим к следующему
                                    return;
                                }
                            }
                        }
                    }
                    // Если квадрат проверен и в нем нет повторяющихся чисел, окрашиваем его в зелёный цвет
                    ColorBox(boxRow, boxCol, Color.Green);
                }
            }
        }


        // Метод ColorRow изменяет цвет фона всех текстовых полей в указанной строке судоку на заданный цвет
        private void ColorRow(int row, Color color)
        {
            // Перебираем все столбцы в заданной строке
            for (int col = 0; col < 9; col++)
            {
                // Устанавливаем цвет фона текстового поля в заданный цвет
                sudokuTextBoxes[row, col].BackColor = color;
            }
        }


        // Метод ColorColumn изменяет цвет фона всех текстовых полей в указанном столбце судоку на заданный цвет
        private void ColorColumn(int col, Color color)
        {
            // Перебираем все строки в заданном столбце
            for (int row = 0; row < 9; row++)
            {
                // Устанавливаем цвет фона текстового поля в заданный цвет
                sudokuTextBoxes[row, col].BackColor = color;
            }
        }


        // Метод ColorBox изменяет цвет фона всех текстовых полей в указанном квадрате 3x3 судоку на заданный цвет
        private void ColorBox(int boxRow, int boxCol, Color color)
        {
            // Перебираем все строки и столбцы внутри указанного квадрата 3x3
            for (int row = boxRow * 3; row < boxRow * 3 + 3; row++)
            {
                for (int col = boxCol * 3; col < boxCol * 3 + 3; col++)
                {
                    // Устанавливаем цвет фона каждого текстового поля в указанный цвет
                    sudokuTextBoxes[row, col].BackColor = color;
                }
            }
        }


        // Обработчик события нажатия на кнопку "Проверить судоку"
        private void btnCheckSudoku_Click(object sender, EventArgs e)
        {
            // Создание резервной копии текущего состояния судоку
            // Это необходимо для возможности отмены изменений, если проверка не пройдена
            backupSudoku = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // Преобразование текста из текстового поля в целочисленное значение и сохранение в резервной копии
                    backupSudoku[i, j] = int.Parse(sudokuTextBoxes[i, j].Text);
                }
            }
            // Вызов методов проверки корректности заполнения строк, столбцов и блоков 3x3 судоку
            CheckRowsAndColor();
            CheckColumnsAndColor();
            CheckBoxesAndColor();
        }


        // Обработчик события нажатия на кнопку "Отменить изменения"
        private void btnUndoChanges_Click(object sender, EventArgs e)
        {
            // Проверяем, была ли создана резервная копия судоку
            if (backupSudoku != null)
            {
                // Восстанавливаем судоку из резервной копии
                // Перебираем все ячейки судоку и заменяем их содержимое на соответствующие значения из резервной копии
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        // Преобразуем числовые значения из резервной копии в строки и устанавливаем их в текстовые поля судоку
                        sudokuTextBoxes[i, j].Text = backupSudoku[i, j].ToString();
                    }
                }
            }
        }



        // Обработчик события нажатия на кнопку "Сбросить цвета"
        private void btnResetColors_Click(object sender, EventArgs e)
        {
            // Цикл по всем ячейкам судоку, для каждой ячейки вызывается метод ColorAllSudokuBlocks,
            // который сбрасывает цвета блоков 3x3 судоку до исходных значений
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    ColorAllSudokuBlocks();
                }
            }
            // После сброса цветов ячеек судоку, отображается сообщение пользователю, информирующее о том,
            // что цвета были успешно сброшены до исходного состояния
            MessageBox.Show("Цвета сброшены до исходного состояния!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
