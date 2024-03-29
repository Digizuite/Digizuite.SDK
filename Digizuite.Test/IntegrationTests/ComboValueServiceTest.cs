﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Digizuite.Metadata;
using Digizuite.Models.Metadata.Values;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    [TestFixture]
    public class ComboValueServiceTest : IntegrationTestBase<ComboValueService>
    {
        private const int MetaFieldLabelId = 51732;

        [Test]
        public async Task CanCrud()
        {
            var guid = Guid.NewGuid().ToString();

            var def = await Service.CreateComboValue(MetaFieldLabelId, new ComboValueDefinition
            {
                Text = guid,
                Value = guid,
            });

            await CheckCreatedValue(guid, def.Id);
            
            var secondGuid = Guid.NewGuid().ToString();

            def.Text = secondGuid;
            def.Value = secondGuid;

            await Service.UpdateComboValue(def);

            await CheckCreatedValue(secondGuid, def.Id);
            
            await Service.DeleteComboValue(def.Id);

            for (var i = 0; i < 60; i++)
            {
                var values = await Service.GetComboValuesForMetaField(MetaFieldLabelId);

                var exists = values.Items.Any(cv => cv.Id == def.Id);

                if (!exists)
                {
                    Assert.Pass();
                }

                await Task.Delay(1000);
            }
            Assert.Fail("Combo value was not deleted");
        }

        private async Task CheckCreatedValue(string expected, int id)
        {
            
            var values = await Service.GetComboValuesForMetaField(MetaFieldLabelId);

            var match = values.Items.Single(i => i.Id == id);
            Assert.That(match.Text, Is.EqualTo(expected));
            Assert.That(match.Value, Is.EqualTo(expected));
        }
    }
}