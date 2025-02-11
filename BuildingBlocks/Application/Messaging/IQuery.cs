﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messaging;
public interface IQuery<TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}