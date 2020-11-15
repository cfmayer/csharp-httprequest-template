FROM openfaas/of-watchdog:0.8.1-x86_64 as watchdog
#FROM amd64/ubuntu:20.04 

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal-amd64 as build

COPY --from=watchdog /fwatchdog /usr/bin/fwatchdog
RUN chmod +x /usr/bin/fwatchdog 

# Supress collection of data.
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# Optimize for Docker builder caching by adding projects first.

RUN mkdir -p /home/app/function
WORKDIR /home/app/function
COPY ./function/function.csproj  .

WORKDIR /home/app/src/
COPY ./root.csproj  .
RUN dotnet restore ./root.csproj

COPY .  .

RUN dotnet publish -c release -o published

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal-amd64

COPY --from=build /usr/bin/fwatchdog .

# Create a non-root user
RUN addgroup --system app \
    && adduser --system --ingroup app app

WORKDIR /home/app/
COPY --from=build /home/app/src/published .
RUN chown app:app -R /home/app

USER app

ENV fprocess="dotnet ./root.dll"
ENV cgi_headers="true"
ENV mode="http"
ENV upstream_url="http://localhost:5000"
EXPOSE 8080

HEALTHCHECK --interval=3s CMD [ -e /tmp/.lock ] || exit 1

CMD ["fwatchdog"]