global using System.ComponentModel.DataAnnotations;

global using MediatR;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;

global using BudgetManager.Data;
global using BudgetManager.Models;
global using BudgetManager.Mappers;
global using BudgetManager.Services;
global using BudgetManager.Repositories;

global using BudgetManager.Dto;
global using BudgetManager.Dto.Income;
global using BudgetManager.Dto.Pattern;
global using BudgetManager.Dto.Transaction;
global using BudgetManager.Dto.MonthPattern;
global using BudgetManager.Dto.RecurringTransaction;

global using BudgetManager.Features.Incomes.Commands;
global using BudgetManager.Features.Incomes.Queries;
global using BudgetManager.Features.MonthPatterns.Commands;
global using BudgetManager.Features.MonthPatterns.Queries;
global using BudgetManager.Features.Patterns.Commands;
global using BudgetManager.Features.Patterns.Queries;
global using BudgetManager.Features.Statistics.Queries;
global using BudgetManager.Features.RecurringTransactions.Commands;
global using BudgetManager.Features.RecurringTransactions.Queries;
global using BudgetManager.Features.Transactions.Commands;
global using BudgetManager.Features.Transactions.Queries;

global using BudgetManager.Exceptions;
global using BudgetManager.Exceptions.PatternExceptions;
global using BudgetManager.Exceptions.IncomeExceptions;
global using BudgetManager.Exceptions.TransactionExceptions;