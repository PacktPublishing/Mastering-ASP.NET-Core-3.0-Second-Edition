﻿namespace chapter01
{
    public class MyService : IMyService
    {
        private readonly IMyOtherService _other;

        public MyService(IMyOtherService other)
        {
            this._other = other;
        }

        public void MyOperation()
        {
            //do something
        }
    }
}