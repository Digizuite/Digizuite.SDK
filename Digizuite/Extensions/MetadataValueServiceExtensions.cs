﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Digizuite.Metadata;
using Digizuite.Metadata.RequestModels;
using Digizuite.Metadata.ResponseModels;
using Digizuite.Metadata.ResponseModels.Metadata;
using Digizuite.Metadata.ResponseModels.MetaFields;
using Digizuite.Metadata.ResponseModels.Properties;
using Digizuite.Models.Metadata;
using Digizuite.Models.Metadata.Fields;
using Digizuite.Models.Metadata.Values;

using ComboValue = Digizuite.Models.Metadata.Values.ComboValue;
using TreeValue = Digizuite.Models.Metadata.Values.TreeValue;

namespace Digizuite.Extensions
{
    public static class MetadataValueServiceExtensions
    {
        public static async Task<List<Field>> GetAllMetadata(this IMetadataValueService service,
            GetMetadataRequest request,
            CancellationToken cancellationToken = default, string? accessKey = null)
        {
            var response = await service.GetRawMetadata(request, cancellationToken, accessKey);

            var labelValues = response.Values
                .GroupBy(v => v.LabelId)
                .ToDictionary(g => g.Key, g => g.ToList());

            return response.Fields.SelectMany(field =>
            {

                var responsesValues = new List<MetadataResponse>();

                foreach (var label in field.Labels)
                {
                    if (labelValues.TryGetValue(label.Value.LabelId, out var resValues))
                    {
                        responsesValues.AddRange(resValues);
                    }
                }

#pragma warning disable 612
                if (!responsesValues.Any() && labelValues.TryGetValue(field.LabelId, out var responses))
                {
                    responsesValues.AddRange(responses);
                }
#pragma warning restore 612

                return responsesValues!.Select(value =>
                {
                    Field parsedField = (field, value) switch
                    {
                        (IntMetaFieldResponse f, IntMetadataResponse v) => ParseIntMetafield(f, v),
                        (StringMetaFieldResponse f, StringMetadataResponse v) => ParseStringMetafield(f, v),
                        (BitMetaFieldResponse f, BitMetadataResponse v) => ParseBitMetafield(f, v),
                        (NoteMetaFieldResponse f, NoteMetadataResponse v) => ParseNoteMetafield(f, v),
                        (DateTimeMetaFieldResponse f, DateTimeMetadataResponse v) => ParseDateTimeMetafield(f, v),
                        (MultiComboValueMetaFieldResponse f, MultiComboValueMetadataResponse v) =>
                            ParseMultiComboMetafield(f, v),
                        (ComboValueMetaFieldResponse f, ComboValueMetadataResponse v) => ParseComboMetafield(f, v),
                        (MasterItemReferenceMetaFieldResponse f, MasterItemReferenceMetadataResponse v) =>
                            ParseMasterItemReferenceMetafield(f, v),
                        (SlaveItemReferenceMetaFieldResponse f, SlaveItemReferenceMetadataResponse v) =>
                            ParseSlaveItemReferenceMetafield(f, v),
                        (FloatMetaFieldResponse f, FloatMetadataResponse v) => ParseFloatMetafield(f, v),
                        (LinkMetaFieldResponse f, LinkMetadataResponse v) => ParseLinkMetafield(f, v),
                        (TreeMetaFieldResponse f, TreeMetadataResponse v) => ParseTreeMetafield(f, v),
                        (EditMultiComboValueMetaFieldResponse f, EditMultiComboValueMetadataResponse v) =>
                            ParseEditMultiComboMetafield(f, v),
                        (EditComboValueMetaFieldResponse f, EditComboValueMetadataResponse v) => ParseEditComboMetafield(f, v),
                        _ => throw new ArgumentOutOfRangeException(nameof(field),
                            $"Unknown metafield/metadata value type combination. Got {field.GetType()}/{value.GetType()}")
                    };

                    return parsedField;
                });
            }).ToList();
        }

        private static TField PopulateField<TField>(MetaFieldResponse fieldResponse, MetadataResponse valueResponse,
            TField field)
            where TField : Field
        {
            var label = fieldResponse.Labels.SingleOrDefault(l => l.Value.LabelId == valueResponse.LabelId);
            field.FieldItemId = fieldResponse.ItemId;
            field.MetafieldId = fieldResponse.MetafieldId;
#pragma warning disable 612
            field.LabelId = label.Value?.LabelId ?? fieldResponse.LabelId;
            field.LanguageId = label.Value?.LanguageId ?? fieldResponse.LanguageId;
            field.Label = label.Value?.Label ?? fieldResponse.Label;
#pragma warning restore 612
            field.ReadOnly = fieldResponse.Readonly;
            field.Required = fieldResponse.Required;
            field.SortIndex = fieldResponse.SortIndex;
            field.AutoTranslate = fieldResponse.AutoTranslate;
            field.VisibilityMetaFieldId = fieldResponse.VisibilityMetaFieldId;
            field.VisibilityRegex = fieldResponse.VisibilityRegex;
            field.TargetItemId = valueResponse.ItemId;
            field.System = fieldResponse.System;
            field.RestrictToAssetType = fieldResponse.RestrictToAssetType;
            field.UploadTagName = fieldResponse.UploadTagName;
            return field;
        }

        /// <summary>
        /// Gets the specified metafield
        /// </summary>
        /// <param name="service"></param>
        /// <param name="assetItemId">The itemId of the asset to load metadata for</param>
        /// <param name="metafieldLabelId">The labelId of the field to load</param>
        /// <param name="cancellationToken"></param>
        public static async Task<BitMetafield> GetBitMetafield(this IMetadataValueService service, int assetItemId,
            int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) = await service.GetSingleField<BitMetaFieldResponse, BitMetadataResponse>(assetItemId,
                metafieldLabelId, cancellationToken, () => new BitMetadataResponse(false));

            return ParseBitMetafield(field, value);
        }

        private static BitMetafield ParseBitMetafield(BitMetaFieldResponse field, BitMetadataResponse value)
        {
            return PopulateField(field, value, new BitMetafield
            {
                Value = value.Value
            });
        }

        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<ComboMetafield> GetComboMetafield(this IMetadataValueService service, int assetItemId,
            int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) = await service.GetSingleField<ComboValueMetaFieldResponse, ComboValueMetadataResponse>(
                assetItemId, metafieldLabelId, cancellationToken,
                () => new ComboValueMetadataResponse(null));

            return ParseComboMetafield(field, value);
        }

        private static ComboMetafield ParseComboMetafield(ComboValueMetaFieldResponse field,
            ComboValueMetadataResponse value)
        {
            return PopulateField(field, value, new ComboMetafield
            {
                ViewType = field.ViewType,
                Value = value.Value == null ? null : ParseComboValueValue(value.Value)
            });
        }

        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<EditComboMetafield> GetEditComboMetafield(this IMetadataValueService service,
            int assetItemId, int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) =
                await service.GetSingleField<EditComboValueMetaFieldResponse, EditComboValueMetadataResponse>(
                    assetItemId, metafieldLabelId, cancellationToken,
                    () => new EditComboValueMetadataResponse(null));


            return ParseEditComboMetafield(field, value);
        }

        private static EditComboMetafield ParseEditComboMetafield(EditComboValueMetaFieldResponse field,
            EditComboValueMetadataResponse value)
        {
            return PopulateField(field, value, new EditComboMetafield
            {
                Value = value.Value == null ? null : ParseEditComboValueValue(value.Value)
            });
        }

        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<MultiComboMetafield> GetMultiComboMetafield(this IMetadataValueService service,
            int assetItemId, int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) =
                await service.GetSingleField<MultiComboValueMetaFieldResponse, MultiComboValueMetadataResponse>(
                    assetItemId, metafieldLabelId, cancellationToken,
                    () => new MultiComboValueMetadataResponse(new()));


            return ParseMultiComboMetafield(field, value);
        }

        private static MultiComboMetafield ParseMultiComboMetafield(MultiComboValueMetaFieldResponse field,
            MultiComboValueMetadataResponse value)
        {
            return PopulateField(field, value, new MultiComboMetafield
            {
                Value = value.Values.Select(ParseComboValueValue).ToList()
            });
        }

        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<EditMultiComboMetafield> GetEditMultiComboMetafield(this IMetadataValueService service,
            int assetItemId, int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) = await service
                .GetSingleField<EditMultiComboValueMetaFieldResponse, EditMultiComboValueMetadataResponse>(
                    assetItemId, metafieldLabelId, cancellationToken,
                    () => new EditMultiComboValueMetadataResponse(new()));


            return ParseEditMultiComboMetafield(field, value);
        }

        private static EditMultiComboMetafield ParseEditMultiComboMetafield(EditMultiComboValueMetaFieldResponse field,
            EditMultiComboValueMetadataResponse value)
        {
            return PopulateField(field, value, new EditMultiComboMetafield
            {
                Value = value.Values.Select(ParseEditComboValueValue).ToList()
            });
        }

        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<IntMetafield> GetIntMetafield(this IMetadataValueService service, int assetItemId,
            int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) = await service.GetSingleField<IntMetaFieldResponse, IntMetadataResponse>(assetItemId,
                metafieldLabelId, cancellationToken, () => new IntMetadataResponse(null));

            return ParseIntMetafield(field, value);
        }

        private static IntMetafield ParseIntMetafield(IntMetaFieldResponse field, IntMetadataResponse value)
        {
            return PopulateField(field, value, new IntMetafield
            {
                Value = value.Value
            });
        }

        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<StringMetafield> GetStringMetafield(this IMetadataValueService service,
            int assetItemId,
            int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) = await service.GetSingleField<StringMetaFieldResponse, StringMetadataResponse>(
                assetItemId, metafieldLabelId, cancellationToken, () => new StringMetadataResponse(""));

            return ParseStringMetafield(field, value);
        }

        private static StringMetafield ParseStringMetafield(StringMetaFieldResponse field, StringMetadataResponse value)
        {
            return PopulateField(field, value, new StringMetafield
            {
                MaxLength = field.MaxLength,
                Value = value.Value
            });
        }

        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<LinkMetafield> GetLinkMetafield(this IMetadataValueService service, int assetItemId,
            int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) = await service.GetSingleField<LinkMetaFieldResponse, LinkMetadataResponse>(assetItemId,
                metafieldLabelId, cancellationToken, () => new LinkMetadataResponse(null));

            return ParseLinkMetafield(field, value);
        }

        private static LinkMetafield ParseLinkMetafield(LinkMetaFieldResponse field, LinkMetadataResponse value)
        {
            return PopulateField(field, value, new LinkMetafield
            {
                Value = value.Value
            });
        }

        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<FloatMetafield> GetFloatMetafield(this IMetadataValueService service, int assetItemId,
            int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) = await service.GetSingleField<FloatMetaFieldResponse, FloatMetadataResponse>(
                assetItemId, metafieldLabelId, cancellationToken, () => new FloatMetadataResponse(null));

            return ParseFloatMetafield(field, value);
        }

        private static FloatMetafield ParseFloatMetafield(FloatMetaFieldResponse field, FloatMetadataResponse value)
        {
            return PopulateField(field, value, new FloatMetafield
            {
                Value = value.Value
            });
        }


        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<NoteMetafield> GetNoteMetafield(this IMetadataValueService service, int assetItemId,
            int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) = await service.GetSingleField<NoteMetaFieldResponse, NoteMetadataResponse>(assetItemId,
                metafieldLabelId, cancellationToken, () => new NoteMetadataResponse(""));

            return ParseNoteMetafield(field, value);
        }

        private static NoteMetafield ParseNoteMetafield(NoteMetaFieldResponse field, NoteMetadataResponse value)
        {
            return PopulateField(field, value, new NoteMetafield
            {
                MaxLength = field.MaxLength,

                Value = value.Value
            });
        }

        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<DateTimeMetafield> GetDateTimeMetafield(this IMetadataValueService service,
            int assetItemId, int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) = await service.GetSingleField<DateTimeMetaFieldResponse, DateTimeMetadataResponse>(
                assetItemId, metafieldLabelId, cancellationToken, () => new DateTimeMetadataResponse(null));

            return ParseDateTimeMetafield(field, value);
        }

        private static DateTimeMetafield ParseDateTimeMetafield(DateTimeMetaFieldResponse field,
            DateTimeMetadataResponse value)
        {
            return PopulateField(field, value, new DateTimeMetafield
            {
                SubType = field.ViewType,
                Value = value.Value
            });
        }


        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<TreeMetafield> GetTreeMetafield(this IMetadataValueService service, int assetItemId,
            int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) = await service.GetSingleField<TreeMetaFieldResponse, TreeMetadataResponse>(assetItemId,
                metafieldLabelId, cancellationToken, () => new TreeMetadataResponse(new()));

            return ParseTreeMetafield(field, value);
        }

        private static TreeMetafield ParseTreeMetafield(TreeMetaFieldResponse field, TreeMetadataResponse value)
        {
            return PopulateField(field, value, new TreeMetafield
            {
                RecursiveToRoot = field.SelectToRoot,
                Value = value.Values.Select(v => new TreeValue
                {
                    Id = v.Id,
                    Label = v.Label,
                    Value = v.OptionValue
                }).ToList()
            });
        }

        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<MasterItemReferenceMetafield> GetMasterItemReferenceMetafield(
            this IMetadataValueService service, int assetItemId,
            int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) =
                await service.GetSingleField<MasterItemReferenceMetaFieldResponse, MasterItemReferenceMetadataResponse>(
                    assetItemId, metafieldLabelId, cancellationToken,
                    () => new MasterItemReferenceMetadataResponse(new()));

            return ParseMasterItemReferenceMetafield(field, value);
        }

        private static MasterItemReferenceMetafield ParseMasterItemReferenceMetafield(
            MasterItemReferenceMetaFieldResponse field, MasterItemReferenceMetadataResponse value)
        {
            return PopulateField(field, value, new MasterItemReferenceMetafield
            {
                MaxItems = field.MaxCount,
                RefType = field.ItemType,
                Value = value.Items.Select(v => new ItemReferenceOption
                {
                    Label = v.Title,
                    BaseId = v.BaseId,
                    ItemId = v.ItemId
                }).ToList()
            });
        }

        /// <inheritdoc cref="GetBitMetafield"/>
        public static async Task<SlaveItemReferenceMetafield> GetSlaveItemReferenceMetafield(
            this IMetadataValueService service, int assetItemId,
            int metafieldLabelId, CancellationToken cancellationToken = default)
        {
            var (field, value) =
                await service.GetSingleField<SlaveItemReferenceMetaFieldResponse, SlaveItemReferenceMetadataResponse>(
                    assetItemId, metafieldLabelId, cancellationToken,
                    () => new SlaveItemReferenceMetadataResponse(new()));

            return ParseSlaveItemReferenceMetafield(field, value);
        }

        private static SlaveItemReferenceMetafield ParseSlaveItemReferenceMetafield(
            SlaveItemReferenceMetaFieldResponse field, SlaveItemReferenceMetadataResponse value)
        {
            return PopulateField(field, value, new SlaveItemReferenceMetafield
            {
                RefType = field.ItemType,
                Value = value.Items.Select(v => new ItemReferenceOption
                {
                    Label = v.Title,
                    BaseId = v.BaseId,
                    ItemId = v.ItemId
                }).ToList()
            });
        }


        private static ComboValue ParseComboValueValue(ComboValueResponse value)
        {
            return new()
            {
                Label = value.Label,
                OptionValue = value.OptionValue,
                Id = value.Id
            };
        }

        private static ComboValue ParseEditComboValueValue(ComboValueResponse value)
        {
            return new()
            {
                Label = value.Label,
                OptionValue = value.OptionValue,
                Id = value.Id
            };
        }

        private static async Task<(TField, TData)> GetSingleField<TField, TData>(this IMetadataValueService service,
            int assetItemId, int metafieldLabelId,
            CancellationToken cancellationToken, Func<TData> getDefaultValue)
            where TField : MetaFieldResponse
            where TData : MetadataResponse
        {
            var requestBody = new GetMetadataRequest
            {
                ItemIds = new List<int> { assetItemId },
                LabelIds = new HashSet<int> { metafieldLabelId }
            };

            var response = await service.GetRawMetadata(requestBody, cancellationToken).ConfigureAwait(false);

            var field = (TField)response.Fields.Single();

            TData value;
            if (response.Values.Count == 0)
            {
                value = getDefaultValue();
                value.LabelId = metafieldLabelId;
                value.ItemId = assetItemId;
            }
            else
                value = (TData)response.Values.Single();

            return (field, value);
        }


        public static Task UpdateFields(this IMetadataValueService service,
            int assetItemId, string? accessKey = null, CancellationToken cancellationToken = default,
            params Field[] fields)
        {
            return service.UpdateFields(new[] { assetItemId }, accessKey, cancellationToken, fields);
        }
    }
}
