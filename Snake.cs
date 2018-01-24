using System;
using System.Collections.Generic;
using System.Drawing;

namespace Snake
{
    public enum GameStatus
    {
        None,
        Finish,
        GameOver
    }

    public class Snake
    {
        public enum MoveDirection { None, Left, Right, Up, Down };
        public enum FieldType { Empty, Border, Tail, Food };
        public enum FoodType { Food1, Food2, Food3 };
        public bool IsAlive = false;
        public int Points;
        public FoodType CurrentFood;
        public const int xSize = 32, ySize = 20;
        public FieldType[,] Pool;
        private LinkedList<Point> tail;
        private MoveDirection moveDirection = MoveDirection.None;
        private Random r;

        public Snake()
        {
            this.Pool = new FieldType[xSize, ySize];
            this.tail = new LinkedList<Point>();
            InitRandom();
            InitPool();
        }

        public void Start()
        {
            InitPool();
            InitTail();
            MakeFoodPoint();
            moveDirection = MoveDirection.None;
            IsAlive = true;
            Points = 0;
        }

        public void Move(MoveDirection direction)
        {
            Point p_next = Point.Empty;

            if (!IsAlive)
            {
                return;
            }

            if (!((moveDirection == MoveDirection.Left && direction == MoveDirection.Right) ||
                  (moveDirection == MoveDirection.Right && direction == MoveDirection.Left) ||
                  (moveDirection == MoveDirection.Up && direction == MoveDirection.Down) ||
                  (moveDirection == MoveDirection.Down && direction == MoveDirection.Up)))
            {
                moveDirection = direction;
            }

            if (CheckColision(moveDirection))
            {
                IsAlive = false;
                return;
            }

            p_next = GetNextHeadPoint(moveDirection);

            if (Pool[p_next.X, p_next.Y] == FieldType.Food)
            {
                Points++;
                MakeFoodPoint();
            }
            else
            {
                foreach (Point p in tail) Pool[p.X, p.Y] = FieldType.Empty;
                tail.RemoveLast();
            }

            tail.AddFirst(p_next);

            foreach (Point p in tail) Pool[p.X, p.Y] = FieldType.Tail;
        }

        private void InitPool()
        {
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    Pool[x, y] = (x == 0 || y == 0 || x == xSize - 1 || y == ySize - 1) ? FieldType.Border : FieldType.Empty;
                }
            }
        }

        private void InitRandom()
        {
            DateTime dt = DateTime.Now;
            this.r = new Random((dt.Second * dt.Minute) + dt.Hour);
        }

        private void InitTail()
        {
            tail.Clear();
            tail.AddFirst(new Point(3, 5));
            tail.AddFirst(new Point(4, 5));
            tail.AddFirst(new Point(5, 5));
            tail.AddFirst(new Point(6, 5));
            tail.AddFirst(new Point(7, 5));
            tail.AddFirst(new Point(8, 5));

            foreach (Point p in tail) Pool[p.X, p.Y] = FieldType.Tail;
        }

        private void MakeFoodPoint()
        {
            int x = 0, y = 0;

            while (Pool[x, y] != FieldType.Empty)
            {
                x = r.Next(xSize);
                y = r.Next(ySize);
            }
            Pool[x, y] = FieldType.Food;
            CurrentFood = (FoodType)r.Next(3);
        }

        private Point GetNextHeadPoint(MoveDirection direction)
        {
            Point p = Point.Empty;

            switch (direction)
            {
                case MoveDirection.Left:
                    p = new Point(tail.First.Value.X - 1, tail.First.Value.Y);
                    break;
                case MoveDirection.Right:
                    p = new Point(tail.First.Value.X + 1, tail.First.Value.Y);
                    break;
                case MoveDirection.Up:
                    p = new Point(tail.First.Value.X, tail.First.Value.Y - 1);
                    break;
                case MoveDirection.Down:
                    p = new Point(tail.First.Value.X, tail.First.Value.Y + 1);
                    break;

            }
            return p;
        }

        private bool CheckColision(MoveDirection direction)
        {
            Point p = GetNextHeadPoint(direction);
            return Pool[p.X, p.Y] == FieldType.Border || Pool[p.X, p.Y] == FieldType.Tail;
        }
    }
}