using AutoBogus;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;
using Xapien.Services.Interfaces;

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

        public static XapienThread CreateMockXapienThread(IProcessRunner ProcRunner, int StepCount){
            Faker faker = new Faker();
            XapienThread xapienThread = new XapienThread(faker.Random.String2(8));
            xapienThread.SetProcessRunner(ProcRunner);
            for (int i = 0; i < StepCount; i++) {
                xapienThread.AddStep((CreateMockStep()));
            }

            return xapienThread;
        }
    }
}
