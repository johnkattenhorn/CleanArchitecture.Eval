namespace Applicita.AAF2.Domain.Contributor.Specifications;

public class ContributorByIdSpec : Specification<Contributor>
{
  public ContributorByIdSpec(int contributorId) =>
    Query
        .Where(contributor => contributor.Id == contributorId);
}
