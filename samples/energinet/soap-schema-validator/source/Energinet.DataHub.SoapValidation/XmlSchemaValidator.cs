﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Energinet.DataHub.SoapValidation.Dtos;
using Energinet.DataHub.SoapValidation.Helpers;
using Energinet.DataHub.SoapValidation.Schemas;

namespace Energinet.DataHub.SoapValidation
{
    public class XmlSchemaValidator : IXmlSchemaValidator
    {
        /// <summary>
        /// Validates a xml-stream using all schema collections
        /// </summary>
        /// <param name="stream"><see cref="Stream"/></param>
        /// <returns><see cref="ValidationResult"/></returns>
        /// <exception cref="InvalidOperationException">No document definition or none/multiple schemas matched the definition</exception>
        public Task<ValidationResult> ValidateStreamAsync(Stream stream)
            => ValidateStreamAsync(stream, false);

        /// <summary>
        /// Validates a xml-stream
        /// </summary>
        /// <param name="stream"><see cref="Stream"/></param>
        /// <param name="traverseSubDefinitions">true - if sub definitions should be searched</param>
        /// <returns><see cref="ValidationResult"/></returns>
        /// <exception cref="InvalidOperationException">No document definition or none/multiple schemas matched the definition</exception>
        public async Task<ValidationResult> ValidateStreamAsync(Stream stream, bool traverseSubDefinitions)
        {
            var position = stream.Position;
            var controller = new ValidationController();

            try
            {
                var definition = await XmlUtil.GetDocumentIdentificationAsync(stream);

                var schemaDefinition = SchemaCollection.Find(traverseSubDefinitions, definition).Single();

                var settings = GetXmlReaderSettings(controller, schemaDefinition.CreateXmlSchemaSet());
                using var validationContext = controller.GetValidationContext(settings);
                using XmlReader xmlReader = XmlReader.Create(stream, settings);

                try
                {
                    await ReadDocumentAsync(xmlReader, schemaDefinition, controller);

                    var problems = controller.GetProblems();
                    return problems.Any()
                    ? new ValidationResult(false, RejectionReason.DoesNotRespectSchema, problems)
                    : new ValidationResult(true, RejectionReason.None, problems);
                }
                catch (XmlSchemaException)
                {
                    return new ValidationResult(false, RejectionReason.SchemasUnavailable, controller.GetProblems());
                }
                catch (XmlException)
                {
                    return new ValidationResult(false, RejectionReason.InvalidXml, controller.GetProblems());
                }
                catch
                {
                    return new ValidationResult(false, RejectionReason.UnableToValidate, controller.GetProblems());
                }
            }
            catch (InvalidOperationException)
            {
                return new ValidationResult(false, RejectionReason.SchemasUnavailable, controller.GetProblems());
            }
            catch (XmlException)
            {
                return new ValidationResult(false, RejectionReason.InvalidXml, controller.GetProblems());
            }
            finally
            {
                stream.Position = position;
            }
        }

        private static XmlReaderSettings GetXmlReaderSettings(ValidationController controller, XmlSchemaSet schemaSet)
        {
            var settings = new XmlReaderSettings()
            {
                Async = true,
                ValidationType = ValidationType.Schema,
                ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema |
                                  XmlSchemaValidationFlags.ReportValidationWarnings,
                Schemas = schemaSet,
            };

            settings.ValidationEventHandler += (obj, ea) =>
            {
                controller.AddProblem(settings, new ValidationProblem(ea.Message, ea.Severity, ea.Exception.LineNumber, ea.Exception.LinePosition));
            };

            return settings;
        }

        private static bool ShouldSwitchToSubReader(XmlReader xmlReader, SchemaDefinition schemaDefinition)
        {
            if (xmlReader.NodeType == XmlNodeType.Element &&
                xmlReader.NamespaceURI != schemaDefinition.Namespace)
            {
                return schemaDefinition.ContainsSubSchemaDefinition(xmlReader.LocalName, xmlReader.NamespaceURI);
            }

            return false;
        }

        private async Task ReadDocumentAsync(XmlReader xmlReader, SchemaDefinition schemaDefinition, ValidationController context)
        {
            while (await xmlReader.ReadAsync())
            {
                if (ShouldSwitchToSubReader(xmlReader, schemaDefinition))
                {
                    await ReadSubTreeAsync(xmlReader, schemaDefinition, context);
                }
            }
        }

        private async Task ReadSubTreeAsync(XmlReader xmlReader, SchemaDefinition schemaDefinition, ValidationController controller)
        {
            var subSchemaDefinition = schemaDefinition.GetSubSchemaDefinition(xmlReader.LocalName, xmlReader.NamespaceURI);
            var settings = GetXmlReaderSettings(controller, subSchemaDefinition.CreateXmlSchemaSet());

            IXmlLineInfo info = (IXmlLineInfo)xmlReader;
            controller.RemoveProblemsCausedOnSameLineAndPosition(info.LineNumber, info.LinePosition);

            using var subTreeReader = XmlReader.Create(xmlReader.ReadSubtree(), settings);
            using var context = controller.GetValidationContext(settings);
            await ReadDocumentAsync(subTreeReader, subSchemaDefinition, controller);
        }
    }
}
