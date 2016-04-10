using System;

namespace vrpr.Core.Infrastructure
{
    public class Result
    {
        protected Result(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public bool Success { get; private set; }

        public bool Failed => !Success;

        public string Error { get; private set; }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result Fail(string error)
        {
            return new Result(false, error);
        }

        public static Result<T> Ok<T>(T value)
        {
            return Result<T>.Ok(value);
        }

        public static Result<T> Fail<T>(string error)
        {
            return Result<T>.Fail(error);
        } 
    }

    public class Result<T> : Result
    {
        private T _value;

        protected Result(T value) : base(true, string.Empty)
        {
            Value = value;
        }

        protected Result(string error) : base(false, error)
        {
        } 

        public T Value
        {
            get
            {
                if (Failed)
                {
                    throw new NotSupportedException("Can't get value from Failed Result");
                }
                return _value;
            }
            protected set { _value = value; }
        }

        public static Result<T> Ok(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Fail(string error)
        {
            return new Result<T>(error);
        } 
    }
}
