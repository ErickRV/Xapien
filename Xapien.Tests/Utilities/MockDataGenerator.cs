using AutoBogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;

namespace Xapien.Tests.Utilities
{
    internal class MockDataGenerator
    {
        public static Step CreateMockStep() {
            return AutoFaker.Generate<Step>();
        }

        public static StepResult CreateMockStepResult() {
            return AutoFaker.Generate<StepResult>();
        }
    }
}
