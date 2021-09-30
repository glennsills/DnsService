using DNS.Client.RequestResolver;
using DNS.Protocol;
using DNS.Protocol.ResourceRecords;

namespace DnsService
{
    public class SampleRequestResolver : IRequestResolver
    {
        public Task<IResponse> Resolve(IRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            IResponse response = Response.FromRequest(request);

            foreach (var question in response.Questions)
            {
                if (question.Type == RecordType.TXT)
                {
                    response.AnswerRecords.Add(new TextResourceRecord(
                        question.Name, CharacterString.FromString(DateTime.UtcNow.ToString("O"))));
                }
                else
                {
                    response.ResponseCode = ResponseCode.Refused;
                }
            }

            return Task.FromResult(response);
        }
    }
}
