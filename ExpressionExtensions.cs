// MIT License
// 
// Copyright (c) 2021 Vyacheslav Napadovsky
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE. 

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace slavanap.Expressions {

    public static class Expr {

        internal class ExpressionSimplifier : ExpressionVisitor {
            readonly Dictionary<ParameterExpression, Expression> _replaceMap = new Dictionary<ParameterExpression, Expression>();

            static object GetValueOfExpression(Expression exp) {
                switch (exp.NodeType) {
                    case ExpressionType.Constant:
                        return (exp as ConstantExpression).Value;
                    case ExpressionType.MemberAccess: {
                        var memberExpression = exp as MemberExpression;
                        var innerExpression = memberExpression.Expression;
                        var parentValue = innerExpression is null ? null : GetValueOfExpression(innerExpression);
                        if (memberExpression.Member is PropertyInfo propInfo)
                            return propInfo.GetValue(parentValue);
                        else
                            return (memberExpression.Member as FieldInfo).GetValue(parentValue);
                    }
                    default:
                        throw new ArgumentException("The expression must contain only member/property to access sub-expressions", nameof(exp));
                }
            }

            protected override Expression VisitMethodCall(MethodCallExpression node) {
                if (node.Method.ReflectedType == typeof(Expr) && node.Method.Name == nameof(Use)) {
                    LambdaExpression replaceWith = (LambdaExpression)GetValueOfExpression(node.Arguments[0]);
                    if (replaceWith.Parameters.Count != node.Arguments.Count - 1)
                        throw new ArgumentException("Invalid expression supplied", nameof(node));
                    for (int i = 0; i < replaceWith.Parameters.Count; ++i)
                        _replaceMap.Add(replaceWith.Parameters[i], base.Visit(node.Arguments[i + 1]));

                    return base.Visit(replaceWith.Body);
                }
                return base.VisitMethodCall(node);
            }

            protected override Expression VisitParameter(ParameterExpression node) {
                if (_replaceMap.TryGetValue(node, out Expression target))
                    return target;
                return base.VisitParameter(node);
            }
        }


        // Extension methods

        public static TResult Use<TResult>(
            this Expression<Func<TResult>> expression
        ) => expression.Compile()();

        public static TResult Use<T1, TResult>(
            this Expression<Func<T1, TResult>> expression,
            T1 a1
        ) => expression.Compile()(a1);

        #region public static TResult Use<T1, ..., T16, TResult>(this Expression<Func<T1, ..., T16, TResult>> expression, T1 a1, ..., T16 a16)

        public static TResult Use<T1, T2, TResult>(
            this Expression<Func<T1, T2, TResult>> expression,
            T1 a1, T2 a2
        ) => expression.Compile()(a1, a2);

        public static TResult Use<T1, T2, T3, TResult>(
            this Expression<Func<T1, T2, T3, TResult>> expression,
            T1 a1, T2 a2, T3 a3
        ) => expression.Compile()(a1, a2, a3);

        public static TResult Use<T1, T2, T3, T4, TResult>(
            this Expression<Func<T1, T2, T3, T4, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4
        ) => expression.Compile()(a1, a2, a3, a4);

        public static TResult Use<T1, T2, T3, T4, T5, TResult>(
            this Expression<Func<T1, T2, T3, T4, T5, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4, T5 a5
        ) => expression.Compile()(a1, a2, a3, a4, a5);

        public static TResult Use<T1, T2, T3, T4, T5, T6, TResult>(
            this Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6
        ) => expression.Compile()(a1, a2, a3, a4, a5, a6);

        public static TResult Use<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7
        ) => expression.Compile()(a1, a2, a3, a4, a5, a6, a7);

        public static TResult Use<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8
        ) => expression.Compile()(a1, a2, a3, a4, a5, a6, a7, a8);

        public static TResult Use<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9
        ) => expression.Compile()(a1, a2, a3, a4, a5, a6, a7, a8, a9);

        public static TResult Use<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10
        ) => expression.Compile()(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10);

        public static TResult Use<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11
        ) => expression.Compile()(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);

        public static TResult Use<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12
        ) => expression.Compile()(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);

        public static TResult Use<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13
        ) => expression.Compile()(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13);

        public static TResult Use<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13, T14 a14
        ) => expression.Compile()(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14);

        public static TResult Use<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13, T14 a14, T15 a15
        ) => expression.Compile()(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);

        public static TResult Use<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>> expression,
            T1 a1, T2 a2, T3 a3, T4 a4, T5 a5, T6 a6, T7 a7, T8 a8, T9 a9, T10 a10, T11 a11, T12 a12, T13 a13, T14 a14, T15 a15, T16 a16
        ) => expression.Compile()(a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<TResult>>
                               Unfold<TResult>(
                 this Expression<Func<TResult>> expression
        ) =>         (Expression<Func<TResult>>)new ExpressionSimplifier().Visit(expression);

        #region public static Expression<Func<T1, ..., T16, TResult>Unfold<T1, ..., T16, TResult>(Expression<Func<T1, ..., T16, TResult> expression)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, TResult>>
                               Unfold<T1, TResult>(
                 this Expression<Func<T1, TResult>> expression
        ) =>         (Expression<Func<T1, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, TResult>>
                               Unfold<T1, T2, TResult>(
                 this Expression<Func<T1, T2, TResult>> expression
        ) =>         (Expression<Func<T1, T2, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, TResult>>
                               Unfold<T1, T2, T3, TResult>(
                 this Expression<Func<T1, T2, T3, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, TResult>>
                               Unfold<T1, T2, T3, T4, TResult>(
                 this Expression<Func<T1, T2, T3, T4, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, TResult>>
                               Unfold<T1, T2, T3, T4, T5, TResult>(
                 this Expression<Func<T1, T2, T3, T4, T5, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, T5, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, TResult>>
                               Unfold<T1, T2, T3, T4, T5, T6, TResult>(
                 this Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, T5, T6, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>
                               Unfold<T1, T2, T3, T4, T5, T6, T7, TResult>(
                 this Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>
                               Unfold<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
                 this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>
                               Unfold<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
                 this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>
                               Unfold<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
                 this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>
                               Unfold<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
                 this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>
                               Unfold<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
                 this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>
                               Unfold<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
                 this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>
                               Unfold<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
                 this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>)new ExpressionSimplifier().Visit(expression);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>
                               Unfold<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
                 this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>)new ExpressionSimplifier().Visit(expression);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>>
                               Unfold<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
                 this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>> expression
        ) =>         (Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>>)new ExpressionSimplifier().Visit(expression);

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<TResult>>
                                  New<TResult>(
                      Expression<Func<TResult>> expression) => expression;

        #region static Expression<Func<T1, ..., T16, TResult>>New<T1, ..., T16, TResult>(Expression<Func<T1, ..., T16, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, TResult>>
                                  New<T1, TResult>(
                      Expression<Func<T1, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, TResult>>
                                  New<T1, T2, TResult>(
                      Expression<Func<T1, T2, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, TResult>>
                                  New<T1, T2, T3, TResult>(
                      Expression<Func<T1, T2, T3, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, TResult>>
                                  New<T1, T2, T3, T4, TResult>(
                      Expression<Func<T1, T2, T3, T4, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, TResult>>
                                  New<T1, T2, T3, T4, T5, TResult>(
                      Expression<Func<T1, T2, T3, T4, T5, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, TResult>>
                                  New<T1, T2, T3, T4, T5, T6, TResult>(
                      Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>
                                  New<T1, T2, T3, T4, T5, T6, T7, TResult>(
                      Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>
                                  New<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
                      Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>
                                  New<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
                      Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>
                                  New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
                      Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>
                                  New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
                      Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>
                                  New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
                      Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>
                                  New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
                      Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>
                                  New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
                      Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>
                                  New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
                      Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression) => expression;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>>
                                  New<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
                      Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>> expression) => expression;

        #endregion
    }

}
