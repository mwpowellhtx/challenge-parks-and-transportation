#ifndef CHALLENGES_PARSERS_HPP
#define CHALLENGES_PARSERS_HPP

#include <vector>
#include <string>

namespace challenges {

    extern int to_int(std::string const & s);

    extern std::vector<int> to_ints(std::vector<std::string> const & values);
}

#endif // CHALLENGES_PARSERS_HPP
