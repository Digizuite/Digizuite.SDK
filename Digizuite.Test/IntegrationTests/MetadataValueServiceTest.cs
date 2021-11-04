using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Metadata;
using Digizuite.Models.Metadata.Fields;
using Digizuite.Models.Metadata.Values;
using Digizuite.Test.TestUtils;
using Digizuite.Extensions;
using Digizuite.Metadata.RequestModels.UpdateModels;
using Digizuite.Metadata.RequestModels.UpdateModels.Values;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Digizuite.Test.IntegrationTests
{
    [TestFixture]
    public class MetadataValueServiceTest : IntegrationTestBase
    {
        private const int TestAssetItemId = 10224;

        private async Task TestChanges<TValue, TField>(int fieldItemId,
            Func<int, int, CancellationToken, Task<TField>> load,
            TValue first, TValue second)
            where TField : Field<TValue>
        {
            var fieldService = ServiceProvider.GetRequiredService<IMetaFieldService>();

            var metafields = await fieldService.GetAllMetaFields();

            var labelId = metafields.First(mf => mf.ItemId == fieldItemId).LabelId;

            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();

            var field = await load(TestAssetItemId, labelId, CancellationToken.None);

            Assert.That(field.TargetItemId, Is.EqualTo(TestAssetItemId));
            field.Value = first;

            await service.UpdateFields(TestAssetItemId, fields: field);

            var updated = await load(TestAssetItemId, labelId, CancellationToken.None);

            Assert.That(updated.TargetItemId, Is.EqualTo(TestAssetItemId));
            Compare(updated.Value, first);

            field.Value = second;

            await service.UpdateFields(TestAssetItemId, fields: field);

            updated = await load(TestAssetItemId, labelId, CancellationToken.None);
            Assert.That(updated.TargetItemId, Is.EqualTo(TestAssetItemId));

            Compare(updated.Value, second);
        }

        private void Compare<T>(T actual, T expected)
        {
            if (actual is IEnumerable)
            {
                Assert.That(actual, Is.EquivalentTo(expected as IEnumerable));
            }
            else
            {
                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public async Task CanLoadAndUpdateField_Bit()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();
            await TestChanges(10271, service.GetBitMetafield, false, true);
        }

        [Test]
        public async Task CanLoadAndUpdateField_Combo()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();
#pragma warning disable 8631
            await TestChanges(10273, service.GetComboMetafield, new ComboValue()
#pragma warning restore 8631
            {
                Label = "A",
                OptionValue = "A",
                Id = 51285,
            }, new ComboValue
            {
                Label = "B",
                OptionValue = "B",
                Id = 51284,
            });
        }

        [Test]
        public async Task CanLoadAndUpdateField_DateTime()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();
            // We need the explicit type to make the type nullable
            // And we need to truncate them because DC only goes to second precision
            DateTime? first = DateTime.MinValue.Truncate();
            DateTime? second = DateTime.MaxValue.Truncate();

            await TestChanges(10275, service.GetDateTimeMetafield, first, second);
        }

        [Test]
        public async Task CanLoadAndUpdateField_EditCombo()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();

            var second = Guid.NewGuid().ToString();
#pragma warning disable 8631
            await TestChanges(10277, service.GetEditComboMetafield, new ComboValue
#pragma warning restore 8631
            {
                Label = "A",
                OptionValue = "A",
                Id = 51282
            }, new ComboValue
            {
                Label = second,
                OptionValue = second,
            });
        }

        [Test]
        public async Task CanLoadAndUpdateField_EditMultiCombo()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();

            var first = Guid.NewGuid().ToString();
            var second = Guid.NewGuid().ToString();
            await TestChanges(10279, service.GetEditMultiComboMetafield, new List<ComboValue>
            {
                new ComboValue
                {
                    Label = "A",
                    OptionValue = "A",
                },
                new ComboValue
                {
                    Label = first,
                    OptionValue = first,
                }
            }, new List<ComboValue>
            {
                new ComboValue
                {
                    Label = "B",
                    OptionValue = "B",
                },
                new ComboValue
                {
                    Label = second,
                    OptionValue = second,
                }
            });
        }

        [Test]
        public async Task CanLoadAndUpdateField_Float()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();
            await TestChanges(10281, service.GetFloatMetafield, (double?)1d, 2d);
        }

        [Test]
        public async Task CanLoadAndUpdateField_Int()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();
            await TestChanges(10283, service.GetIntMetafield, (int?)1, 2);
        }

        [Test]
        public async Task CanLoadAndUpdateField_Link()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();
#pragma warning disable 8631
            await TestChanges(10285, service.GetLinkMetafield, "https://digizuite.com", "https://digizuite.com/foo");
#pragma warning restore 8631
        }

        [Test]
        public async Task CanLoadAndUpdateField_String()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();
            await TestChanges(10295, service.GetStringMetafield, "foo", "bar");
        }

        [Test]
        public async Task CanLoadAndUpdateField_Note()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();
            await TestChanges(10291, service.GetNoteMetafield, "foo", "bar");
        }

        [Test]
        public async Task CanLoadAndUpdateField_MultiCombo()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();
            await TestChanges(10287, service.GetMultiComboMetafield, new List<ComboValue>
            {
                new ComboValue
                {
                    Label = "A",
                    Id = 51276,
                    OptionValue = "A",
                },
                new ComboValue
                {
                    Id = 51275,
                    Label = "B",
                    OptionValue = "B",
                }
            }, new List<ComboValue>
            {
                new ComboValue
                {
                    Label = "C",
                    OptionValue = "C",
                    Id = 51274
                }
            });
        }

        [Test]
        public async Task CanLoadAndUpdateField_Tree()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();
            await TestChanges(10297, service.GetTreeMetafield, new List<TreeValue>
            {
                new TreeValue
                {
                    Id = 91,
                    Label = "A",
                    Value = "449657bf-b740-4498-b402-415762356f35"
                },
                new TreeValue
                {
                    Id = 95,
                    Label = "AA",
                    Value = "483fe475-8e54-4621-81c4-15107926335e"
                }
            }, new List<TreeValue>
            {
                new TreeValue
                {
                    Id = 101,
                    Label = "CA",
                    Value = "215ac668-f788-4991-89a9-a1acb976148c"
                }
            });
        }

        [Test]
        public async Task CanLoadAndUpdateField_MasterSlaveItemReference()
        {
            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();
            var masterLabelId = 51688;
            var slaveLabelId = 51690;
            var slaveAssetItemId = 10229;

            var field = await service.GetMasterItemReferenceMetafield(TestAssetItemId, masterLabelId);

            field.Value = new List<ItemReferenceOption>
            {
                new ItemReferenceOption
                {
                    BaseId = 90,
                    ItemId = slaveAssetItemId,
                    Label = "XCAo_gm4kxw",
                }
            };

            await service.UpdateFields(TestAssetItemId, fields: field);

            var updated = await service.GetMasterItemReferenceMetafield(TestAssetItemId, masterLabelId);

            Assert.That(updated.Value, Is.EquivalentTo(field.Value));


            var slave = await service.GetSlaveItemReferenceMetafield(slaveAssetItemId, slaveLabelId);

            Assert.That(slave.Value, Is.EquivalentTo(new List<ItemReferenceOption>
            {
                new ItemReferenceOption
                {
                    BaseId = 85,
                    ItemId = TestAssetItemId,
                    Label = "RvDi7uf9ugI",
                }
            }));

            slave.Value = new List<ItemReferenceOption>();

            await service.UpdateFields(slaveAssetItemId, fields: slave);

            slave = await service.GetSlaveItemReferenceMetafield(slaveAssetItemId, slaveLabelId);
            Assert.That(slave.Value, Is.Empty);
            var master = await service.GetMasterItemReferenceMetafield(TestAssetItemId, masterLabelId);
            Assert.That(master.Value, Is.Empty);
        }

        [Test]
        public async Task MultiMethods()
        {
            const int multiComboLabelId = 51687;
            const int testAssetItemId = 10205;

            var comboA = new ComboValue
            {
                Label = "A",
                Id = 51276,
                OptionValue = "A",
            };
            var comboB = new ComboValue
            {
                Id = 51275,
                Label = "B",
                OptionValue = "B",
            };
            var comboC = new ComboValue
            {
                Label = "C",
                OptionValue = "C",
                Id = 51274
            };

            var service = ServiceProvider.GetRequiredService<IMetadataValueService>();

            // Clear the field
            await service.ApplyUpdate(new[]
            {
                new MultiComboValueMetadataUpdate
                {
                    TargetItemIds = {testAssetItemId},
                    MetaFieldLabelId = multiComboLabelId
                }
            });


            var field = await service.GetMultiComboMetafield(testAssetItemId, multiComboLabelId);
            Assert.That(field.Value, Is.Empty);

            // Set the first value
            await service.ApplyUpdate(new[]
            {
                new MultiComboValueMetadataUpdate
                {
                    TargetItemIds = {testAssetItemId},
                    MetaFieldLabelId = multiComboLabelId,
                    ComboValues =
                    {
                        new ExistingCombo(comboA.Id)
                    }
                }
            });

            field = await service.GetMultiComboMetafield(testAssetItemId, multiComboLabelId);
            Assert.That(field.Value, Has.Exactly(1).Items);
            Assert.That(field.Value, Has.Exactly(1).Items.EqualTo(comboA));


            // Merge
            await service.ApplyUpdate(new[]
            {
                new MultiComboValueMetadataUpdate
                {
                    TargetItemIds = {testAssetItemId},
                    MetaFieldLabelId = multiComboLabelId,
                    UpdateMethod = MultiUpdateMethod.Merge,
                    ComboValues =
                    {
                        new ExistingCombo(comboB.Id)
                    }
                }
            });

            field = await service.GetMultiComboMetafield(testAssetItemId, multiComboLabelId);
            Assert.That(field.Value, Has.Exactly(2).Items);
            Assert.That(field.Value, Is.EquivalentTo(new[] { comboA, comboB }));


            // Unset
            await service.ApplyUpdate(new[]
            {
                new MultiComboValueMetadataUpdate
                {
                    TargetItemIds = {testAssetItemId},
                    MetaFieldLabelId = multiComboLabelId,
                    UpdateMethod = MultiUpdateMethod.Unset,
                    ComboValues =
                    {
                        new ExistingCombo(comboB.Id)
                    }
                }
            });

            field = await service.GetMultiComboMetafield(testAssetItemId, multiComboLabelId);
            Assert.That(field.Value, Has.Exactly(1).Items);
            Assert.That(field.Value, Has.Exactly(1).Items.EqualTo(comboA));

        }
    }
}