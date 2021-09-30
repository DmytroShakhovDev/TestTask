using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.WebAPI.Controllers
{
    public class Result
    {
        public Result()
        {
        }

        public Result(params string[] errors)
        {
            Errors = errors.ToList();
        }

        public List<string> Errors { get; set; } = new List<string>();
        [JsonIgnore]
        public bool HasErrors => Errors != null && Errors.Any();

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public void AddErrors(string[] errors)
        {
            Errors.AddRange(errors);
        }
    }

    public class Result<T> : Result
    {
        [JsonConstructor]
        public Result(T value)
        {
            Value = value;
        }

        public Result(params string[] errors) : base(errors)
        {
        }

        public T Value { get; set; }
    }
}
