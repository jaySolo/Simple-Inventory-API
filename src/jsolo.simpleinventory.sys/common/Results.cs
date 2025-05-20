using System;
using System.Collections.Generic;
using System.Linq;



namespace jsolo.simpleinventory.sys.common
{
    public class Result
    {

        public bool Succeeded { get; }

        public string[] Errors { get; }

        protected Result(bool succeeded, IEnumerable<string> errors) : this(succeeded)
            => Errors = errors.ToArray();

        protected Result(bool succeeded) => this.Succeeded = succeeded;

        public static Result Success => new Result(true);

        public static Result Failure(params string[] errors) => new Result(false, errors);
    }



    public class DataOperationResult : Result, IEquatable<DataOperationResult>
    {        
        public bool IsDataValid { get; }

        public bool? AlreadyExists { get; }

        public bool? HasWarnings { get; }

        public bool? IsPartiallySuccessful { get; }


        protected DataOperationResult(
            bool succeeded,
            bool isModelValid,
            bool? exists = null,
            bool? hasWarnings = null,
            bool? isPartialSuccess = null,
            params string[] errors
        ) : base(succeeded, errors ?? new List<string>().ToArray()) {
            this.IsDataValid = isModelValid;
            this.AlreadyExists = exists;
            this.IsPartiallySuccessful = isPartialSuccess;
            this.HasWarnings = hasWarnings;
        }
        

        public bool Equals(DataOperationResult other)
        {
            var areEqual = true;

            areEqual &= this.IsDataValid.Equals(other.IsDataValid);

            areEqual &= this.Succeeded.Equals(other.Succeeded);

            areEqual &= this.AlreadyExists == null ? other.AlreadyExists == null :
                this.AlreadyExists?.Equals(other.AlreadyExists) == true;

            areEqual &= this.HasWarnings == null ? other.HasWarnings == null :
                this.HasWarnings?.Equals(other.HasWarnings) == true;

            areEqual &= this.IsPartiallySuccessful == null ? other.IsPartiallySuccessful == null :
                this.IsPartiallySuccessful?.Equals(other.IsPartiallySuccessful) == true;

            return areEqual;
        }

        public new static DataOperationResult Success => new DataOperationResult(true, true);


        public new static DataOperationResult Failure(
            params string[] errors
        ) => new DataOperationResult(
            false,
            true,
            errors: errors
        );
        
        public static DataOperationResult NotFound => new DataOperationResult(false, true, exists: false);

        public static DataOperationResult Exists => new DataOperationResult(false, true, exists: true);

        public static DataOperationResult InvalidData => new DataOperationResult(false, false);

        public static DataOperationResult SomeChangesNotSaved => new DataOperationResult(
            false, true, exists: true, isPartialSuccess: true
        );

        public static DataOperationResult CompletedWithWarnings => new DataOperationResult(
            false, true, hasWarnings: true, isPartialSuccess: true
        );
    }



    public class DataOperationResult<T> : DataOperationResult, IEquatable<DataOperationResult<T>>
        where T : class
    {
        public T Data { get; }
        
        
        protected DataOperationResult(
            bool succeeded,
            bool isModelValid,
            T data,
            bool? exists = null,
            bool? hasWarnings = null,
            bool? isPartialSuccess = null,
            params string[] errors
        ) : base(
            succeeded,
            isModelValid,
            exists,
            hasWarnings,
            isPartialSuccess,
            errors ?? new List<string>().ToArray()
        )  => Data = data;


        public bool Equals(DataOperationResult<T> other)
        {
            var areEqual = true;

            areEqual &= this.IsDataValid.Equals(other.IsDataValid);

            areEqual &= this.Succeeded.Equals(other.Succeeded);

            areEqual &= this.AlreadyExists == null ? other.AlreadyExists == null :
                this.AlreadyExists?.Equals(other.AlreadyExists) == true;

            areEqual &= this.HasWarnings == null ? other.HasWarnings == null :
                this.HasWarnings?.Equals(other.HasWarnings) == true;

            areEqual &= this.IsPartiallySuccessful == null ? other.IsPartiallySuccessful == null :
                this.IsPartiallySuccessful?.Equals(other.IsPartiallySuccessful) == true;

            return areEqual;
        }


        public new static DataOperationResult<T> Success(T data) => new DataOperationResult<T> (
            succeeded: true,
            isModelValid: true,
            data: data
        );


        public new static DataOperationResult<T> NotFound => new DataOperationResult<T>(
            succeeded: false,
            isModelValid: true,
            data: null,
            exists: false
        );


        public new static DataOperationResult<T> Exists => new DataOperationResult<T> (
            succeeded: false,
            isModelValid: true,
            data: null,
            exists: true
        );


        public static DataOperationResult<T> InvalidData => new DataOperationResult<T> (
            succeeded: false,
            isModelValid: false,
            data: null,
            exists: false
        );


        public new static DataOperationResult<T> SomeChangesNotSaved => new DataOperationResult<T>(
            succeeded: false,
            isModelValid: true,
            data: null,
            exists: true,
            isPartialSuccess: true
        );


        public new static DataOperationResult<T> Failure(
            params string[] errors
        ) => new DataOperationResult<T>(
            succeeded: false,
            isModelValid: true,
            data: null,
            errors: errors
        );
    }
}
