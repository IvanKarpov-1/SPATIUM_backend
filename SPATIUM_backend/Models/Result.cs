﻿namespace SPATIUM_backend.Models;

public class Result<T>
{
	public bool IsSuccess { get; set; }
	public T? Value { get; set; }
	public string? Error { get; set; }
	public bool ValidationError { get; set; }

	public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
	public static Result<T> Failure(string error) => new() { IsSuccess = false, Error = error };
	public Result<T> IsValidationError()
	{
		ValidationError = true;
		return this;
	}
}