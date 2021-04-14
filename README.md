# Welcome to the Green Energy Hub

- [Our Mission Statement](#our-mission-statement)
- [The Architectural Principles](#the-archtitectural-principles-behind-green-energy-hub)
- [The Green Energy Hub Domains](#the-green-energy-hub-domains)
- [Getting started](#getting-started)
- [Business value](#business-value)
- [Project Sponsors and contributors](#project-sponsors-and-contributors)

## Our Mission Statement

### Energinet engages with Green Energy Hub in Open Source with a desire to accelerate the green transition

*"The movement must be seen in relation to Energinet's vision and strategy, as well as the political ambitions of Europe - in particular the Danish ambitions for 100% renewable energy in the electricity system by 2030 and a climate-neutral society by 2050*  
*We are continuously working to design, maintain, develop, and expand energy systems, that will make it possible to use renewable energy - nationally and globally.*  
*We want to use digitalization as a way to accelerate a market-driven transition towards a sustainable and efficient energy system, and we will build the foundation for both new players (market participants) and business models through digital partnerships.*  
*We want to create access to relevant data and insights from the energy market and thereby pushing our goals and direction.*  
*We believe that across TSOs, market players (participants) and technology companies, we can support reaching the goals through an open and equal collaboration*
*Our approach is therefore driven by a strategic belief that digital development is essential, and societal value creation will follow as we engage and open up.*
*Therefore, we have chosen an approach that we believe best supports our overall vision. We are actively working to maximize value creation both nationally and internationally by choosing a system approach, where we will open and partner with others to minimize development costs and maximize deployment. We see an opportunity to reduce the cost of software, but also to greatly increase the quality and pace of development through open collaborations. It is a method and approach that we see is increasingly gaining prominence in TSO cooperation.*
*Energinet is not an IT company, and therefore we do not sell systems, services or operate other TSOs. Our core business is clear, and it must be maintained, but we can contribute to the acceleration (of change?) inside and outside the country through the methods and tools we use – in this movement we see open source as an important tool."*

**Martin Lundoe, CEO of Datahub, Part of the Danish TSO, Energinet.**

## The Architectural Principles behind Green Energy Hub

*By implementing Domain Driven Design, we divide Green Energy Hub, into smaller independent domains. This gives the possibility only to use the domains that are needed for other participants in the Open-Source community. As the domains sends event when data changes, and the other domains listen on these events to have the most updated version of data. This means that when only using one or few domains, data missing from the other domains must be either excluded or retrieved elsewhere.*

*This gives a good offset for open collaboration on smaller parts of Green Energy Hub, and new domains can be added by contributors, to extend the Green Energy Hub’s functionality, when needed to accelerate the green transition.*

**Martin Hansen, Solution Architect - Green Energy Hub**
<br>

## The Green Energy Hub Domains

The Green Energy Hub system consist of several different domains. There are 2 different types of domains:

- A domain that is responsible for handling a subset of business processes.
- A domain that is responsible for handling an internal part of the system (Like log accumulation, secret sharing or similar).

Below is a list of these domains, and the business flows they are responsible for.

- Business Process Domains
    - [Metering Point](https://github.com/Energinet-DataHub/geh-metering-point)
        - Create metering point
        - Submission of master data – grid company
        - Close down metering point
        - Connection of metering point with status new
        - Change of settlement method
        - Disconnection and reconnection of metering point
        - Meter management
        - Update production obligation
        - Request for service from grid company
    - [Aggregations](https://github.com/Energinet-DataHub/geh-aggregations)
        - Submission of calculated energy time series
        - Request for historical data
        - Request for calculated energy time series
        - Aggregation of wholesale services
        - Request for aggregated tariffs
        - Request for settlement basis
    - [Time Series](https://github.com/Energinet-DataHub/geh-timeseries)
        - Submission of metered data for metering point
        - Send missing data log
        - Request for metered data for a metering point
    - [Charges](https://github.com/Energinet-DataHub/geh-charges)
        - Request for aggregated subscriptions or fees
        - Update subscription price list
        - Update fee price list
        - Update tariff price list
        - Request price list
        - Settlement master data for a metering point – subscription, fee and tariff links
        - Request for settlement master data for metering point
    - [Market Roles](https://github.com/Energinet-DataHub/geh-market-roles)
        - Change of supplier
        - End of supply
        - Managing an incorrect change of supplier
        - Move-in
        - Move-out
        - Incorrect move
        - Submission of customer master data by balance supplier
        - Initiate cancel change of supplier by customer
        - Change of supplier at short notice
        - Mandatory change of supplier for metering point
        - Submission of contact address from grid company
        - Change of BRP for energy supplier
    - [Data Requests](https://github.com/Energinet-DataHub/geh-market-roles)
        - Master data request
- System Domains
    - [Shared Resources](https://github.com/Energinet-DataHub/geh-shared-resources)
        - Secrets handling
        - DataBricks workspace
    - [Validation Reports](https://github.com/Energinet-DataHub/geh-validation-reports)
        - Log accumulation for all domains
    - [Post Office](https://github.com/Energinet-DataHub/geh-post-office)
        - Messaging service for outbound messages
    - [API Gateway](https://github.com/Energinet-DataHub/geh-api-gateway)
        - Authentication and routing

## Business Value

To know more about the Green Energy Hub business value, please read [this](./docs/executive-start.md).

## Getting started

To get started utilizing the Green Energy Hub, please read [this](./docs/tech-start.md).

## Project Sponsors and contributors

<img src="./images/energinet.png" alt="Energinet" style="width: 250px; height: auto;" />
<br />
<img src="./images/microsoft.png" alt="Microsoft" style="width: 250px; height: auto;" />
