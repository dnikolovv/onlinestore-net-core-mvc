namespace OnlineStore.IntegrationTests.Infrastructure.TagHelpers
{
    using FakeItEasy;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using OnlineStore.Infrastructure.TagHelpers;
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PageLinkTagHelperTests
    {
        public void CorrectlyRendersAnchorTags(SliceFixture fixture)
        {
            // Arrange
            var tagHelper = GetTagHelper(1, 10, 30);
            var context = GetTagHelperContext();
            var output = GetTagHelperOutput();

            // Act
            tagHelper.Process(context, output);

            // Assert
            var expectedContent = "<a href=\"\">1</a><a href=\"\">2</a><a href=\"\">3</a>";
            output.Content.GetContent().ShouldBe(expectedContent);
        }
        
        public void CorrectlyAppliesStyling(SliceFixture fixture)
        {
            // Arrange
            var tagHelper = GetTagHelper(1, 10, 30);

            tagHelper.PageClassesEnabled = true;
            tagHelper.PageClass = "btn";
            tagHelper.PageClassSelected = "btn-selected";
            tagHelper.PageClassNormal = "btn-default";

            var context = GetTagHelperContext();
            var output = GetTagHelperOutput();

            // Act
            tagHelper.Process(context, output);

            // Assert
            var expectedContent = "<a class=\"btn-selected btn\" href=\"\">1</a><a class=\"btn-default btn\" href=\"\">2</a><a class=\"btn-default btn\" href=\"\">3</a>";
            output.Content.GetContent().ShouldBe(expectedContent);
        }

        private static TagHelperOutput GetTagHelperOutput()
        {
            return new TagHelperOutput(
                "pagination",
                new TagHelperAttributeList(),
                (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent(string.Empty);
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
        }

        private static TagHelperContext GetTagHelperContext()
        {
            return new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));
        }

        private static PageLinkTagHelper GetTagHelper(int page, int itemsPerPage, int totalItems)
        {
            // Arrange
            return new PageLinkTagHelper(A.Fake<IUrlHelperFactory>())
            {
                PageModel = new ViewModels.PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = itemsPerPage,
                    TotalItems = totalItems
                }
            };
        }
    }
}
